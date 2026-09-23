# Windows V12.4 — release activated

The owner confirmed successful testing on the shop computers and explicitly approved public activation on 23 September 2026. This change publishes the website download and stable update metadata; it does not modify the uploaded installer.

## Published package
- Release tag: `V12.4`.
- Asset: `LK_Technologie_Pro_V12_4_Setup.exe`.
- Asset ID: `584171689`.
- Bytes: `196113272`.
- SHA-256: `9f1eb1d858004b7926737538d07f26716abdc5190e1f9fe092743b490ec0da7f`.
- Landing page: `https://lktechnologie.com/download-v12-4.html`.
- Stable manifest: `https://lktechnologie.com/updates/update.json`.
- Minimum version in the manifest remains `12.2`; the existing download-host allowlist is unchanged.

The full installer download, hash, Windows PE headers and installer version resources were verified by run `35890832879` without executing the installer. FileVersion is 12.4.0.0, ProductVersion is 12.4.0, Authenticode status is NotSigned. That automated check is not a claim of independent testing of shop operations; functional approval was supplied by the owner.

## Website implementation
- `_includes/lk-home-content.html` remains the original homepage base with mobile 1.0.25, banner, media and progressive guide logic.
- `index.html` composes that base with `_includes/windows-v12-4-features.html` and updates Windows-specific version/link strings during the Jekyll build. Edit the release wrapper/features for release-specific text, and the base include for general UI changes. Do not enable `.nojekyll` while this structure is used.
- The obsolete V12.4 preview insert was removed. Existing V12.3 release/page/assets remain accessible but are no longer the homepage's Windows call-to-action.
- The homepage download routes through the official-domain landing page with `?start=1`; the landing page requests the binary from the GitHub release and keeps a manual download link. Direct visits without that query do not auto-download.
- Android 1.0.25 links, the mobile banner and guide pagination are preserved.

## Future asset replacement
Never replace a release binary without updating and verifying its size and hash in the manifest and landing page. Prefer a new release version. Do not reuse hashes from previous installers.
