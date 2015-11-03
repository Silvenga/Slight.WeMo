# Slight.WeMo
Let there be light!

## Running

Under **Windows**, run the binary as an Administrator. If you want to run under a standard account use the following:

```
netsh http add urlacl url=http://+:9000/ user=DOMAIN\username
```

Under **Linux**, just run the binary. Make sure Mono is installed.

## How to use?

Open your broswer at:

```
http://{host}:9000/help
```

For example if the server is running locally:

```
http://localhost:9000/help
```
