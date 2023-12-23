#!/bin/bash

# this script will generate a javascript file based on a .env that is passed in
# the generated script will inject a 'configs' object into the window which
# contains all keys of the passed in .env file and either:
# - their current value, extracted from the environment (as a string)
# - null if the current value in the environment has a length of zero

# input parameters:
# $1    .env file to extract the keys from
# $2    path to write the generated javascript file to

function create_config_js {
  # assign readable names to the input parameters
  local env_file=${1:-}
  local destination_path=${2:-}

  # verify the file exists before continuing
  echo "trying env file: ${env_file}"
  if [ -z "$env_file" ] || [ ! -e "$env_file" ]; then
      echo "Source file '$env_file' does not exist"
      exit 1
  fi

  echo "output will be written to: ${destination_path}"

  # assign readable names to checks to use later on
  # we'll use these to skip empty & comment lines
  local is_comment='^[[:space:]]*#'
  local is_blank='^[[:space:]]*$'

  # set up a variable to contain the generated content
  # declare it's a generated file in a comment
  OUTPUT="// generated"
  # open the javascript object
  OUTPUT="$OUTPUT\nwindow.configs = {"

  # loop over all lines in the input file
  # make sure the last one is included as well even if it's not an empty newline!
  while IFS="" read -r line || [ -n "$line" ]; do
    # skip comments & empty lines
    [[ $line =~ $is_comment ]] && continue
    [[ $line =~ $is_blank ]] && continue

    # extract the key (the part before '=')
    key=$(echo "$line" | cut -d '=' -f 1)
    # get the value for this key from the environment
    key_value_in_env="${!key:-}" # either the value or an empty string
    # prepare the value to use in the javascript object
    # if the value in the environment has length 0, use null
    # otherwise, wrap the value in ' to make it a string
    [[ -z "${key_value_in_env}" ]] && value='null' || value="'${key_value_in_env}'"

    # build a string to contain the key & value of the property (and indent it to make it look nice)
    property="  ${key}: ${value},"
    # append to the output
    OUTPUT="$OUTPUT\n$property"
  done < <(cat "${env_file}")

  # close the javascript object
  OUTPUT="$OUTPUT\n};\n"

  # write the generated output to a file
  printf "$OUTPUT" > "$destination_path";
}

# use .env and public/config.js as defaults
create_config_js "${1:-.env}" "${2:-"public/config.js"}";
