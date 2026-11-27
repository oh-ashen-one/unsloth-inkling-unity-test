# Unsloth Inkling Unity Test

Local Unity 6 project wired so a locally running **Inkling 2 Mini** can drive the Unity Editor through [MCP for Unity](https://github.com/CoplayDev/unity-mcp) (the same Unity bridge used in Unsloth's engines guide).

This repo is the Mac Studio test bed for [Unsloth's 2026-11-26 announcement](https://x.com/UnslothAI/status/2126322382239649959): a local LLM controlling Unity to create assets with no human clicking around in the editor.

## Source links

- Unsloth GitHub: https://github.com/unslothai/unsloth
- Unsloth engines / MCP guide (Unity section): https://unsloth.ai/docs/basics/mcp/mcp-engines
- Inkling 2 Mini run-locally guide: https://unsloth.ai/docs/models/inkling-2-mini
- Inkling 2 Mini GGUF (official Unsloth quants): https://huggingface.co/unsloth/Inkling-2-Mini-GGUF
- MCP for Unity package: `https://github.com/CoplayDev/unity-mcp.git?path=/MCPForUnity#main`

## What's already set up on the Mac Studio

| Piece | Status |
| --- | --- |
| Unity Editor | **6000.3.20f1** (`/Applications/Unity/Hub/Editor/6000.3.20f1`) |
| Project path | `/Users/midir/Projects/unsloth-inkling-unity-test` |
| MCP for Unity | added via Git URL in `Packages/manifest.json` |
| HTTP MCP bridge | port **8080**, auto-starts on Editor load (`Assets/Editor/UnslothInklingMcpBootstrap.cs`) |
| LM Studio MCP client | `~/.lmstudio/mcp.json` → `http://127.0.0.1:8080/mcp` (copy also in `mcp.json`) |
| Local model | **AtomicChat Inkling 2 Mini Uncensored Q8_0** (downloaded 2026-11-26 5:17 PM ET) |
| Model path | `~/.lmstudio/models/AtomicChat/Inkling-2-Mini-Uncensored-GGUF/Inkling-2-Mini-Uncensored-Q8_0.gguf` (28.46 GB) + `dspark-Inkling-2-Mini-Q8_0.gguf` + `mmproj-Inkling-2-Mini-Uncensored-BF16.gguf` |
| Inference | LM Studio / llama.cpp, identifier `inkling-2-mini`, **max Metal GPU offload**, context 32,768 |
| API | `http://127.0.0.1:1234/v1` (do **not** bind llama-server to 8080 — that's Unity MCP) |

`uv` is already installed (`/opt/homebrew/bin/uv`) and is what MCP for Unity uses to launch its HTTP server. Unsloth Desktop is **not** required; LM Studio is the MCP host.

## Load / unload the model

Ashen streams Mon–Sat 11:00 AM – 1:00 PM ET from this machine. Do not leave the model loaded past **10:50 AM ET** unless you are actively testing, and never touch Streamlabs, OBS, Stream Deck, or Elgato.

```bash
# load onto GPU (Metal), 32k context, OpenAI-compatible server on :1234
lms load atomicchat/inkling-2-mini-uncensored-gguf/inkling-2-mini-uncensored-q8_0.gguf \
  --gpu max --context-length 32768 --identifier inkling-2-mini -y
lms server start -p 1234

# confirm
lms ps
curl -s http://127.0.0.1:1234/api/v0/models

# unload before the stream window
lms unload inkling-2-mini
lms server stop
```

Recommended sampling from the Unsloth Inkling guide: temperature `1.0`, top-p `1.0`, top-k `0`, min-p `0.0`.

## Open Unity and test

1. Open this project in Unity 6000.3.20f1 (Hub, or `open` the project path with that editor).
2. Keep the Editor open. Window → MCP for Unity should show the HTTP server running on port 8080 (status **Connected** once LM Studio has joined; amber means the server is up but no client/editor handshake yet).
3. In LM Studio, load `inkling-2-mini` if it isn't already, make sure Program → MCP has `unityMCP` enabled, and start a chat.
4. Try a first prompt (from the MCP for Unity / Unsloth docs):

> Use Unity MCP to create a professional-quality low-poly palm prefab with a slender curved trunk and an uneven crown of arching fronds, lime above deep green. Frame the whole tree on warm sand against blue sky with soft side lighting. Check its silhouette at thumbnail size, refine if needed, save the prefab, and show the Game view in chat.

Shorter smoke test: `Create a red, blue, and yellow cube in the current scene.`

If port 8080 is already taken, change it in Window → MCP for Unity and update `mcp.json` / `~/.lmstudio/mcp.json` to match.

## Hardware notes

Inkling 2 Mini is a 26.4B MoE (5.4B active) looped transformer, Apache 2.0. The official Unsloth recommended quant is `UD-Q4_K_XL` (~17.6 GB). This project uses the **Q8_0 uncensored GGUF already on disk from Nov 26** (~28.5 GB; ~29 GB resident at 32k context with full GPU offload). The M3 Ultra's 256 GB unified memory has plenty of headroom.
