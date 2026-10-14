using Mono.Cecil;
using Mono.Cecil.Cil;

if (args.Length != 1)
{
    Console.Error.WriteLine("Usage: NovaDiagPatcher.exe <LK Nova.dll>");
    return 2;
}

var target = Path.GetFullPath(args[0]);
if (!File.Exists(target))
{
    Console.Error.WriteLine("Target not found: " + target);
    return 3;
}

var directory = Path.GetDirectoryName(target)!;
var resolver = new DefaultAssemblyResolver();
resolver.AddSearchDirectory(directory);
using var module = ModuleDefinition.ReadModule(target, new ReaderParameters
{
    ReadWrite = false,
    AssemblyResolver = resolver,
    InMemory = true
});

MethodDefinition? handler = null;
foreach (var type in module.Types.SelectMany(Flatten))
{
    foreach (var method in type.Methods)
    {
        if (!method.HasBody) continue;
        if (method.Body.Instructions.Any(i => i.OpCode == OpCodes.Ldstr &&
            i.Operand is string s && s.Contains("Vérifiez l’historique avant de répéter une vente", StringComparison.Ordinal)))
        {
            handler = method;
            break;
        }
    }
    if (handler is not null) break;
}

if (handler is null)
{
    Console.Error.WriteLine("Global Nova desktop exception handler was not found; no file changed.");
    return 4;
}

var eventArg = handler.Parameters.FirstOrDefault(p =>
    p.ParameterType.FullName.Contains("DispatcherUnhandledExceptionEventArgs", StringComparison.Ordinal));
if (eventArg is null)
{
    Console.Error.WriteLine("DispatcherUnhandledExceptionEventArgs parameter not found.");
    return 5;
}

var resolvedEvent = eventArg.ParameterType.Resolve();
var getException = resolvedEvent?.Properties.FirstOrDefault(p => p.Name == "Exception")?.GetMethod;
if (getException is null)
{
    Console.Error.WriteLine("Exception getter could not be resolved.");
    return 6;
}

var writeAllText = typeof(File).GetMethod(nameof(File.WriteAllText), new[] { typeof(string), typeof(string) })!;
var exceptionToString = typeof(Exception).GetMethod(nameof(Exception.ToString), Type.EmptyTypes)!;
var il = handler.Body.GetILProcessor();
var first = handler.Body.Instructions[0];

var appBaseGetter = typeof(AppContext).GetProperty(nameof(AppContext.BaseDirectory))!.GetMethod!;
var pathCombine = typeof(Path).GetMethod(nameof(Path.Combine), new[] { typeof(string), typeof(string) })!;

var injected = new[]
{
    Instruction.Create(OpCodes.Call, module.ImportReference(appBaseGetter)),
    Instruction.Create(OpCodes.Ldstr, "nova-desktop-exception.txt"),
    Instruction.Create(OpCodes.Call, module.ImportReference(pathCombine)),
    Instruction.Create(OpCodes.Ldarg, eventArg),
    Instruction.Create(OpCodes.Callvirt, module.ImportReference(getException)),
    Instruction.Create(OpCodes.Callvirt, module.ImportReference(exceptionToString)),
    Instruction.Create(OpCodes.Call, module.ImportReference(writeAllText))
};
foreach (var ins in injected) il.InsertBefore(first, ins);

foreach (var instruction in handler.Body.Instructions)
{
    if (instruction.OpCode != OpCodes.Ldstr || instruction.Operand is not string text) continue;
    if (text.Contains("Une erreur est survenue. Vérifiez l’historique", StringComparison.Ordinal))
        instruction.Operand = "Une erreur d’interface est survenue. Détails enregistrés dans : app\\nova-desktop-exception.txt";
    else if (text.Contains("حدث خطأ. تحقق من سجل المبيعات", StringComparison.Ordinal))
        instruction.Operand = "حدث خطأ في الواجهة. تم حفظ التفاصيل في: app\\nova-desktop-exception.txt";
    else if (text.Contains("An error occurred. Check sales history", StringComparison.Ordinal))
        instruction.Operand = "A UI error occurred. Details were saved to: app\\nova-desktop-exception.txt";
}

var backup = target + ".before-diagnostic";
if (!File.Exists(backup)) File.Copy(target, backup, false);
var temp = target + ".diag.tmp";
module.Write(temp);
File.Move(temp, target, true);

Console.WriteLine("PATCH_OK");
Console.WriteLine("Target: " + target);
Console.WriteLine("Backup: " + backup);
Console.WriteLine("Diagnostic log after repro: " + Path.Combine(directory, "nova-desktop-exception.txt"));
File.WriteAllText(Path.Combine(directory, "DIAGNOSTIC_PATCH_INSTALLED.txt"),
    "Nova desktop diagnostic patch installed at " + DateTime.Now.ToString("O") + Environment.NewLine +
    "Target: " + target + Environment.NewLine +
    "Expected log after reproduction: " + Path.Combine(directory, "nova-desktop-exception.txt"));
return 0;

static IEnumerable<TypeDefinition> Flatten(TypeDefinition root)
{
    yield return root;
    foreach (var nested in root.NestedTypes)
        foreach (var item in Flatten(nested))
            yield return item;
}
