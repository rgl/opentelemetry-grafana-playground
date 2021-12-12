# About

This is a [OpenTelemetry](https://github.com/open-telemetry/opentelemetry-dotnet)/[Grafana](https://github.com/grafana/grafana)/[Tempo](https://github.com/grafana/tempo) playground.

# Usage (Ubuntu 20.04)

```bash
# create the environment defined in docker-compose.yml
# and leave it running in the background.
docker compose up -d --build

# show running containers.
docker compose ps

# show logs.
docker compose logs

# destroy the environment.
docker compose down --remove-orphans
```

# Notes

* .NET uses the [Activity class](https://docs.microsoft.com/en-us/dotnet/api/system.diagnostics.activity?view=net-6.0) to encapsulate the [W3C Trace Context](https://www.w3.org/TR/trace-context/).
  * The Activity `Id` property contains to the [W3C `traceparent` header value](https://www.w3.org/TR/trace-context/#traceparent-header).
    * It looks alike `00-98d483b6d0e3a6d012b11e23737faa50-6ac18089ab13c12e-01`.
    * It has four fields: `version`, `trace-id`, `parent-id` (aka `span-id`), and `trace-flags`.

# Reference

* [W3C Trace Context](https://www.w3.org/TR/trace-context/).
