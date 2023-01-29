# ClamAVHttpProxy

A simple HTTP wrapper for [ClamAV](https://www.clamav.net/), using [nClam](https://github.com/tekmaven/nClam) for communication with the ClamAV TCP socket. Authorisation for the API is currently implemented using API keys.

## Running

The easiest way to run this currently is with docker-compose. An example docker-compose file is provided [here](.docker/docker-compose.yml).

To run as a standalone container, pointing at an existing instance of ClamAV, the following can be used:

```bash
docker run -d -e ClamAV__Host=clamav.example.com -e ClamAV__Port=3310 -e Auth__ValidApiKeys__0=ChangeMe -p 80:3311/tcp --restart unless-stopped --name clamavhttpproxy nathanawhitworth/clamavhttpproxy:0.0.1
```

## Scanning

One endpoint is currently exposed (`POST /scan/raw`) which allows scanning the raw body of an HTTP request.

HTTPie:

```bash
http POST localhost:3311/scan/raw X-API-Key:ChangeMe @file.txt
```

cURL:

```bash
curl -X POST -H "X-API-Key: ChangeMe" --data "@file.txt" localhost:3311/scan/raw
```

This returns `0` if the uploaded file is clean and `1` if any malware is detected. The response header(s) `X-Detected` will include the name(s) of the detected malware, as reported by ClamAV.
