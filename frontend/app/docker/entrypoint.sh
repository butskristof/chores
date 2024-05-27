#!/bin/bash

# generate a config.js file with the current runtime environment variables
./create_config_js.sh .env html/config.js

exec "$@"