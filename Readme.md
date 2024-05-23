
# Appsettings

```powershell
op inject -f -i appsettings.tpl.json -o appsettings.json
```

# Tailwindcss

```powershell
npm run css:build
```

# Resharper CLI

```
inspectcode -f="xml" --output="inspectcode.xml" --project="WebApp" "AwsWorkmailAliasesDotNet.sln"
```

# Build

```shell
docker -H "ssh://rpi@192.168.1.242" build -t vadviktor.xyz/aws-workmail-aliases:2.0.0 -f Dockerfile .
```

# Run

```shell
docker \
  -H "ssh://rpi@192.168.1.242" \
  run \
  --detach \
  --name aws-workmail-aliases \
  --restart=always \
  -p 0.0.0.0:3010:8080/tcp \
  vadviktor.xyz/aws-workmail-aliases:2.0.0
```