regex='(.*)-cleaned.dll'
for f in *.dll; do
    [[ $f =~ $regex ]] && mv "$f" "${BASH_REMATCH[1]}.dll"
done