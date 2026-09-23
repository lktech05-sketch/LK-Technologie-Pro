# Windows V12.4 — publication checklist

Status: PREPARATION ONLY. No V12.4 installer or stable app-update manifest is published by this change.

## Upload

Create a NEW GitHub Release, do not rename or overwrite V12.3.
- Tag: `V12.4` (capital V).
- Target: `main` (this repository hosts the website and binary distribution).
- Title: `LK.Technologie Pro V12.4`.
- Asset: `LK_Technologie_Pro_V12_4_Setup.exe`, from `installer_output` in the approved Windows build.
- Upload in the release binaries area, not in `assets/mobile`, not using Add file / Upload files.
- Save draft while Windows/server/client acceptance is incomplete. Do not mark as latest stable before approval.

Expected future public asset URL:
`https://github.com/lktech05-sketch/LK-Technologie-Pro/releases/download/V12.4/LK_Technologie_Pro_V12_4_Setup.exe`

Public landing page prepared now:
`https://lktechnologie.com/download-v12-4.html`
Its V12.4 button is intentionally disabled. Publishing a release does not silently switch stable downloads.

## Before activation

1. Confirm same approved Windows build on server and second PC: product add/edit, sales, supplier purchases, cash operations, returns, atelier, printing, backup and restore. Do not infer approval from source-only tests.
2. Verify the uploaded installer bytes, file size, SHA-256 and embedded version (12.4); verify its signature when available. Never invent a hash or reuse V12.3 values.
3. Publish the validated release, then update the homepage Windows version/CTAs, landing-page status and EXE link together.
4. Update `updates/update.json` with the verified installer URL, actual size and SHA-256 and reviewed V12.4 notes. Inspect updater compatibility before choosing minimum_version. Keep Android 1.0.25 unchanged.
5. Check deployed pages and both downloads, French/Arabic, mobile banner, guides 4 -> 8 -> 9, and the in-app update independently.

## Homepage structure

`_includes/lk-home-content.html` is an exact copy of the prior index blob `0f919b2094cf37fb91181a6dc352373d716cee98`.
`index.html` is a small Jekyll wrapper that inserts `_includes/windows-v12-4-preview.html` before the features section at build time. The generated page remains ordinary static HTML, with the original JS and media links intact.
Edit the base include for future normal homepage changes. Remove the preview insertion after final activation or convert it to the V12.4 release announcement. Do not add `.nojekyll` while this wrapper is in use.

Rollback: restore the index.html blob above; added pages/includes may remain unlinked.
