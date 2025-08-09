# Changelog

## 2025-08-09 — CI: Consolidate WPF workflow & fix artifact path

- Consolidated WPF headless check into the main `windows` workflow and removed the
  separate `.github/workflows/wpf-headless-test.yml` to avoid duplicate CI runs.
  - Commit: `e555e0f` — "ci: consolidate WPF headless check into windows workflow and remove separate workflow"

- Fixed the runtime path used by the WPF check so the downloaded artifact is
  executed from the artifact root (previously used a non-existent nested path).
  - Commit: `bf9804f` — "ci: fix WPF artifact path; run executable from artifact root"

- Workflow run summary:
  - Run `16848729318` (push): failed — WPF job attempted to run
    `./wpf/WpfApp/bin/Release/WpfApp.exe` which did not exist in the artifact layout.
  - Run `16848747314` (push): success — after path fix, the WPF headless check
    ran `./wpf/WpfApp.exe --ci` and reported: "WPF types instantiated successfully (headless check)."

- Notes / next steps:
  - Consider adding an existence check after `actions/download-artifact` such as
    `dir wpf` or a PowerShell `Test-Path` to make the workflow more defensive.
  - Optionally preserve the artifact directory structure or upload a manifest
    so consumers know exact paths inside the artifact.

---

This changelog entry records the CI-focused fixes made on 2025-08-09 to reduce
duplicate workflows and to ensure the WPF headless check runs reliably.

