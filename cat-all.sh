#!/bin/bash

find . -type f \( -name "*.cs" -o -name "*.csproj" -o -name "*.sln" -o -name "*.json" -o -name "*.css" -o -name "*.html" -o -name "*.js" \) \
  -not -path '*/obj/*' \
  -not -path '*/bin/*' \
  -not -path '*/.vs/*' \
  | while read -r file; do
    echo "### $file"
    cat "$file"
    echo -e "\n---\n"
done

