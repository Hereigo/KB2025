```
/init
```

### claude.md 
file provides context to the AI model about the project's structure, conventions, technologies used, commands, architecture, and data flow. It helps reduce the number of tool calls by giving the model upfront information about the codebase, preventing it from needing to make additional queries to understand basic project details.

The entire claude.md file gets added to the assembled prompts and counts toward your context usage. If the file is very large and contains information that isn't used by the model, you'll consume your token budget much faster. It's important to include only relevant information that the model will actually need.

#### /permissions

.claude/settings.json
```json
"permissions": { 
    "allow":[],"deny":[],"ask":[],"default":[] 
}
```

#### /fewer-permission-prompts

The "fewer permission prompts" command analyzes your previous Claude Code sessions, identifies tool calls that you've frequently accepted, and automatically adds them to your permissions settings to reduce repetitive prompting.

#### Advisor mode:

allows you to use a different model (like Opus) specifically for planning tasks while remaining on another model (like Sonnet) for implementation. This lets you leverage the deeper reasoning capabilities of one model for planning without having to manually switch models.

> The effort level may be set too low (medium or low instead of high or max). Before switching to a different model like Opus or Haiku, adjusting the effort level can often resolve the issue.

Setting the effort level to max provides deeper reasoning but results in higher inference costs and more expensive usage. The model may also overthink simple tasks that don't require extensive reasoning.

> The standard context window is 200,000 tokens, but it can be extended up to 1 million tokens.

#### disable model invocation: true

in the skill's front matter prevents the model from automatically invoking the skill, keeping it only available for manual user invocation via slash commands.

#### Skill Creator:

is used to automatically create skills and can also check if a skill actually improves workflow performance. It runs evaluations (evals) on skills to test how the codebase performs with and without the skill, ensuring that added skills actually improve the code rather than just adding unnecessary tokens.

```
Try “list my skills”.
```

#### Using "context_fork" 

in a skill configuration makes the skill run in a sub-agent rather than the main context. The sub-agent runs in parallel with a brand new context, and only the results are returned to the main conversation. This prevents the main context from being cluttered with intermediate tool calls and can improve the quality of model output.

#### A sub-agent 

is a separate loop that runs in its own forked environment with its own context, tools, and system prompt. It runs in the background and only returns results to the main agent, ensuring that its tool call results and intermediate steps don't pollute the main conversation's context.


#### personal agent vs project-specific agent

A personal agent can be reused across multiple projects and repositories, while a project-specific agent is limited to a single project or repository.

> Sub-agents use a lot of tokens because they essentially start from scratch with their own context. They don't have cached prompts at that point, and when multiple sub-agents run in parallel, each one makes its own requests, multiplying token usage significantly.

#### Agent teams 

work against a shared task list created by the main agent. They check this task list continuously, and only when all tasks are complete do they end their work.

Agent teams use a lot more tokens than sub-agents. For most use cases, a sub-agent would be sufficient and more efficient.

Success depends heavily on having an accurate plan and shared task list upfront. Verification still requires manual oversight or built-in checks like tests and images. The team lead agent can notice when teammates quit or are idle and manage them accordingly.

#### Plugins can bundle 
things like skills, agents, hooks, and MCP into something reusable that can be shared with teams or other people.

