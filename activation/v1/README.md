# Fichiers d’activation — LK.Technologie Pro 12.1

Ce dossier sert à publier les enveloppes de licence chiffrées créées par le générateur du propriétaire. Le logiciel les récupère sur le domaine officiel `https://lktechnologie.com/activation/v1/`.

## Ajouter une licence

1. Dans le générateur, créer la licence puis cliquer sur **Exporter [F4]**.
2. Ouvrir le dossier exporté `Licence_…/activation/v1/`.
3. Ajouter ici uniquement le fichier JSON chiffré, en conservant son nom de 64 caractères hexadécimaux suivi de `.json`.
4. Attendre la publication GitHub Pages, puis vérifier que l’URL exacte du fichier renvoie son JSON.
5. Le client peut alors utiliser la clé courte dans le logiciel.

Chaque nouvelle clé a son propre fichier. Le JSON exporté contient uniquement `format`, `nonce` et `ciphertext`. Ne pas publier la clé courte, le fichier `Licence.lklic`, `private_key.txt`, le générateur, ses sources ou le dossier `Licence_…` complet.

`status.json` permet de vérifier la publication du dossier. Ce fichier n’est pas une licence et ne rend aucun code de client actif à lui seul.

## بالعربية

من المولّد اضغط **Exporter [F4]**، ثم افتح `activation/v1` داخل مجلد التصدير. أضف هنا ملف JSON المشفّر بنفس اسمه الأصلي، وانتظر اكتمال نشر الموقع. تتكرر إضافة الملف لكل مفتاح جديد.

يُرفع ملف JSON المشفّر فقط. ملف المفتاح الخاص وملف `Licence.lklic` والمولّد تبقى خارج هذا المستودع العام. ملف `status.json` لفحص وصول الموقع فقط ولا يفعّل أي ترخيص.
