Incoding Framework Core – Architecture Notes

- CQRS messages execute via DispatcherControllerBase (no handlers layer)
- Html.When().Ajax<TQuery>(new { ... }) is canonical pattern
- HashQueryString drives filters and reload via IncChangeUrl
- Queries inherit QueryBaseAsync<T>
- Dispatcher is message executor, not mediator