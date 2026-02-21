# Incoding Framework Core

Incoding Framework Core is a **message-centric CQRS framework** for ASP.NET Core designed for server-driven applications.
It provides a structured execution model built around **Dispatcher**, **Messages**, and a **MetaLanguage (IML) DSL** for UI interaction.

Unlike mediator-style libraries, Incoding executes business logic directly inside messages.
There are **no external handler classes** — execution flows through Dispatcher → Message.

---

# Table of Contents

* Overview
* Core Principles
* Architecture
* Execution Model
* Dispatcher
* Messages (Queries & Commands)
* MetaLanguage DSL
* DispatcherControllerBase
* Unit Of Work & Transactions
* Interceptions Pipeline
* Dispatcher Extensions
* Hash Navigation Model
* Template Rendering
* Testing (Incoding.UnitTests.MSpec)
* Project Structure
* Installation & Configuration
* Mental Model vs Mediator
* Reference Projects

---

# Overview

Incoding Framework Core combines:

* CQRS execution
* server-driven UI updates
* declarative AJAX behaviors
* message-based orchestration

It is optimized for:

* ASP.NET Core MVC
* Razor Views
* Bootstrap UI
* long-lived enterprise applications without SPA complexity

---

# Core Principles

## Message-centric execution

Messages are executable units.

They define:

* input state
* execution logic
* result

There is no separation into request + handler.

## Dispatcher-driven pipeline

Dispatcher coordinates:

* execution
* UnitOfWork lifecycle
* interception hooks
* transaction boundaries

## DSL-first UI

MetaLanguage (IML) enables declarative UI behavior directly inside Razor.

---

# Architecture

```
Razor View (MetaLanguage DSL)
        ↓
DispatcherControllerBase
        ↓
IDispatcher
        ↓
Message.OnExecute(...)
        ↓
Execute / ExecuteAsync
        ↓
ExecuteResult* (business logic)
```

---

# Execution Model

Dispatcher executes messages by calling:

```
message.OnExecute(...)
```

`MessageBase` prepares execution context:

* assigns Repository
* assigns Dispatcher
* injects UnitOfWork repository

Then calls:

```
Execute()
```

Framework base classes redirect execution into:

```
ExecuteResult()
ExecuteResultAsync()
```

Business logic always lives inside `ExecuteResult*`.

---

# Dispatcher

`DefaultDispatcher` is the execution engine.

Responsibilities:

* Groups message parts by MessageExecuteSetting
* Manages UnitOfWork instances
* Executes interception hooks
* Handles flush and commit lifecycle
* Supports nested dispatch calls

Dispatcher never contains domain logic.

Typical usage:

```
dispatcher.Push(command);
dispatcher.Query(query);
```

---

# Messages

## MessageBase

Defines the core execution contract:

```
OnExecute(...)
    → Execute()
```

Messages inherit Repository and Dispatcher during execution.

---

## Commands

Commands represent state changes.

Base class example:

```
CommandBase<T>
```

Execution flow:

```
Execute()
    → ExecuteResult()
```

Commands may return results or operate as void messages.

---

## Queries

Queries retrieve data.

Common base class:

```
QueryBaseAsync<T>
```

Execution flow:

```
ExecuteAsync()
    → Result = await ExecuteResult()
```

Queries typically return DTO arrays or view models.

---

# Dispatcher Extensions

DispatcherExtensions provide fluent helpers:

```
dispatcher.Push(command);
dispatcher.Push(command, setting);

dispatcher.Query(query, cfg => { ... });
```

They configure MessageExecuteSetting and forward execution.

---

# MetaLanguage (IML DSL)

MetaLanguage provides a declarative DSL for UI behavior.

Example:

```
@(Html.When(JqueryBind.InitIncoding | JqueryBind.IncChangeUrl)
    .Ajax<GetJobsSessionsQuery>(new { ... })
    .OnSuccess(dsl =>
    {
        dsl.Self().JQuery.Dom
            .WithTemplateByView("~/Views/.../List.cshtml")
            .Html();
    }))
```

Key characteristics:

* anonymous binding is standard
* DSL controls AJAX behavior
* UI remains server-driven

---

# DispatcherControllerBase

MetaLanguage does not call custom MVC actions.

Requests route through DispatcherControllerBase endpoints:

```
Query(...)
Push(...)
```

These endpoints reconstruct messages and forward them to dispatcher.

---

# Unit Of Work & Transactions

Dispatcher internally manages UnitOfWork instances.

Behavior:

* Messages grouped by execution settings
* Commands trigger flush operations
* Outer dispatch cycle commits once
* Nested dispatch calls reuse existing UnitOfWork

Isolation level and flush behavior depend on MessageExecuteSetting.

---

# Interceptions Pipeline

Dispatcher supports interception hooks:

```
OnBeforeAsync(...)
OnAfterAsync(...)
```

Interceptors allow cross-cutting behavior such as:

* logging
* validation
* auditing
* metrics

---

# Hash Navigation Model

Incoding commonly uses hash-driven UI state.

Example:

```
#!Status=open
```

Receivers listen to:

```
InitIncoding | IncChangeUrl
```

Changing hash reloads content automatically.

---

# Template Rendering

Rendering is template-driven.

Typical pattern:

```
dsl.Self().JQuery.Dom
    .WithTemplateByView("~/Views/.../Tmpl/List.cshtml")
    .Html();
```

Templates are preferred over manual DOM manipulation.

---

# Testing (Incoding.UnitTests.MSpec)

Incoding provides an MSpec-based testing DSL.

Typical workflow:

```
MockQuery<TQuery, TResult>.When(query)
    .StubQuery(...)
    .Execute()
    .ShouldBeIsResult(...)
```

Features:

* message execution without MVC
* stubbing nested queries
* weak equality assertions
* fluent DSL for expectations

Tests mirror real dispatcher execution.

---

# Project Structure

Typical layout:

```
/Commands
/Queries
/Views
    /Tmpl
Controllers/
    DispatcherControllerBase
Tests/
```

---

# Installation & Configuration

Install package:

```
dotnet add package Incoding.Web
```

Configure services:

```
builder.Services.ConfigureIncodingCoreServices();
builder.Services.ConfigureIncodingWebServices();
```

---

# Mental Model vs Mediator

Mediator libraries:

```
Message → Handler → Result
```

Incoding:

```
Message → ExecuteResult → Result
```

The message itself contains execution logic.

---

# Reference Projects

See:

```
Incoding.WebTest80
Incoding.WebTest30.Tests
```

inside the repository for working examples.

---

# When to Use Incoding

Incoding Framework Core is ideal for:

* complex MVC applications
* server-driven AJAX workflows
* CQRS without SPA frameworks
* structured Razor development

---

# License

Refer to repository license for details.
