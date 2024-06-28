# Develop

## Resharper CLI

```shell
inspectcode -f="xml" --output="inspectcode.xml" --project="WebApp" "AwsWorkmailAliasesDotNet.sln"
```

# Deploy

```shell
cd WebApp
```

## Appsettings

```shell
op inject -f -i appsettings.tpl.json -o appsettings.json
```

## Tailwindcss

```shell
npm run css:build
````

## Build

```shell
docker -H "ssh://rpi5-8" build -t "vadviktor.xyz/aws-workmail-aliases:2.2.0" -f Dockerfile .
```

## Run

Stop previous version.

```shell
docker -H "ssh://rpi5-8" stop aws-workmail-aliases
```

Remove previous version.

```shell
docker -H "ssh://rpi5-8" rm aws-workmail-aliases
```

```shell
docker -H "ssh://rpi5-8" run --detach --name aws-workmail-aliases --restart always --network caddy_net -p 0.0.0.0:3010:8080/tcp -e ASPNETCORE_ENVIRONMENT=production "vadviktor.xyz/aws-workmail-aliases:2.2.0"
```
