# OpenAI usage — PowerToys Command Palette Dock

Independent community project; not affiliated with or endorsed by Microsoft or OpenAI. Source is MIT-licensed (see `LICENSE`). This repository intentionally excludes signed MSIX packages, build outputs, and signing certificates. Package installation/signing is local and not covered by the source license.

Shows the remaining **Codex / ChatGPT Work shared allowance** from this computer's Hermes `openai-codex` sign-in. This is not the separate ChatGPT chat message limit or API billing balance.

- Dock title: `AI 92% left` (example). The lowest remaining account window is shown if both 5-hour and weekly windows exist; the subtitle shows each window. If OpenAI returns only a weekly window, the subtitle names that window and its reset time.
- Refresh: every 3 minutes while Command Palette runs. If a refresh fails, the last title is labelled `Last known`; a failed initial read displays `AI usage —` instead of an invented percentage.
- Data: starts the locally installed Hermes launcher with `usage --provider openai-codex --json`. Hermes resolves and refreshes its own account credential; the extension does not store or log credentials and never redeems reset credits.

## Source and package

The Dock uses a solid teal **AI chip** icon loaded from an absolute package asset path. Signed package files are local-only artifacts and are excluded from this repository. The local package identity is `TownsendPrecision.OpenAiUsage`.

To inspect the live usage without the Dock:

```text
hermes usage --provider openai-codex --json
```

To build again, run `dotnet publish OpenAiUsage/OpenAiUsage.csproj -c Release -r win-x64 --self-contained false -o ./publish` and stage/pack/sign a **new package version**; do not overwrite a signed MSIX. The installed Dock band is `uk.townsendprecision.openaiusage.status` in `DockSettings.StartBands`.
