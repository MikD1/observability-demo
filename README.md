# Observability demo

### Run Jaeger

```
docker run --rm --name jaeger \
  -e COLLECTOR_ZIPKIN_HOST_PORT=:9411 \
  -p 6831:6831/udp \
  -p 6832:6832/udp \
  -p 5778:5778 \
  -p 16686:16686 \
  -p 4317:4317 \
  -p 4318:4318 \
  -p 14250:14250 \
  -p 14268:14268 \
  -p 14269:14269 \
  -p 9411:9411 \
  jaegertracing/all-in-one:1.57
```

### Serial calling of services

```mermaid
flowchart LR
  start((S))
  alpha[Alpha Service]
  beta[Beta Service]
  gamma[Gamma Service]
  delta[Delta Service]

  start -->|api/method| alpha
  alpha -->|api/method| beta
  beta -->|api/method| gamma
  beta -->|api/method| delta
```

### Creating Spans

```mermaid
flowchart LR
  start((S))
  m1[Alpha Service - Module 1]
  m2[Alpha Service - Module 2]
  m3[Alpha Service - Module 3]
  m4[Alpha Service - Module 4]

  start -->|api/modular| m1
  m1 --> m2
  m1 --> m3
  m1 --> m4
```

### Kafka

```mermaid
flowchart LR
  start((S))
  kafka[[Kafka]]
  alpha[Alpha Service]
  gamma[Gamma Service]
  delta[Delta Service]

  start -->|api/kafka| alpha
  alpha -->|message| kafka
  kafka -->|message| gamma
  kafka -->|message| delta
```