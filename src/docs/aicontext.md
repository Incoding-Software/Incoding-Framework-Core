# Incoding Framework Core – STRICT AI CONTEXT (Authoritative Guidance)

This document defines **non‑negotiable architectural rules** for projects using **Incoding Framework Core**. 
AI assistants MUST follow these rules when generating examples, explanations, or refactoring suggestions.

This file exists because Incoding is **NOT** a conventional CQRS + Mediator architecture.

---

# PRIMARY EXECUTION MODEL (CRITICAL)

## There are NO external handler classes

Incoding Framework Core does NOT use:

- MediatR-style handlers
- IRequestHandler
- Controller-per-action patterns

Instead:

- Messages (Queries/Commands) are executed directly through Dispatcher infrastructure.
- Execution is message‑centric, not handler‑centric.

Flow:

MetaLanguage (Html.When DSL)
→ Dispatcher URL
→ DispatcherControllerBase (Query/Push actions)
→ IDispatcher
→ Message execution pipeline

AI MUST NEVER introduce "handler" layers unless explicitly requested.

---

# DISPATCHER RULES

## IDispatcher is a message executor

`IDispatcher`:

- Executes message instances directly
- Is NOT a mediator abstraction
- Does NOT require external handler types

Valid nested usage inside any message:

```
await dispatcher.QueryAsync(new SomeQuery());
await dispatcher.PushAsync(new SomeCommand());
```

Nested dispatching is normal and expected.

AI MUST assume dispatcher execution happens inside framework pipeline logic.

---

# CQRS MESSAGE CONVENTIONS

## Queries

- Usually inherit from `QueryBaseAsync<T>`
- Contain filter/state properties
- Return DTO arrays or view models

Invoked from UI via MetaLanguage:

```
.Ajax<TQuery>(new { ... })
```

Invoked internally:

```
dispatcher.QueryAsync(new TQuery());
```

## Commands

- Represent state mutation
- Invoked via `.When("trigger")` chains or dispatcher.PushAsync

---

# METALANGUAGE (IML DSL) — STRICT ASSUMPTIONS

## Anonymous Ajax Binding IS STANDARD

Because of DSL constraints, strongly typed lambdas are NOT always possible.

Correct pattern:

```
.Ajax<TQuery>(new {
    Filter = Selector.Incoding.HashQueryString<TQuery>(r => r.Filter)
})
```

AI MUST NOT attempt to replace this with strongly typed models or alternative AJAX systems.

## DOM Rendering

Preferred rendering:

```
dsl.Self().JQuery.Dom.WithTemplateByView("~/Views/.../Tmpl/List.cshtml").Html();
```

Templates are canonical. Avoid manual DOM construction.

## Navigation + Filtering Model

Incoding UI is HASH‑DRIVEN.

Key rules:

- Filters modify URL hash
- `IncChangeUrl` triggers reload
- Receivers listen to `InitIncoding | IncChangeUrl`

Tabs SHOULD change hash — NOT perform direct AJAX reloads.

---

# CONTROLLER ASSUMPTIONS

AI MUST assume:

- DispatcherControllerBase exists
- Generic Query/Push endpoints exist
- `.Ajax<T>()` routes through Dispatcher URLs

AI MUST NOT generate custom MVC controller actions for each query unless explicitly asked.

---

# UI STACK ASSUMPTIONS (DEFAULTS)

Unless user overrides:

- .NET 8 MVC
- Razor Views
- Incoding Framework Core
- Dispatcher CQRS
- Bootstrap 5
- jQuery present (MetaLanguage dependency)

NOT assumed:

- Blazor
- HTMX
- Alpine
- React

---

# WHAT AI MUST AVOID

Never default to:

- MediatR patterns
- Handler classes
- fetch()/axios replacements for MetaLanguage
- REST-style controller endpoints for queries
- SPA frameworks

If UI interaction exists, prefer MetaLanguage DSL.

---

# REFERENCE IMPLEMENTATION

Canonical reference:

```
Incoding.WebTest80
```

inside the Incoding Framework Core repository.

Patterns in that project override generic ASP.NET conventions.

---

# BEHAVIORAL DIRECTIVE FOR AI

When Incoding Framework Core is mentioned:

1) Assume message-centric execution.
2) Assume DispatcherControllerBase routing.
3) Prefer Html.When DSL solutions.
4) Preserve anonymous Ajax binding style.
5) Do NOT introduce external CQRS libraries.

This document supersedes generic ASP.NET or CQRS assumptions.

