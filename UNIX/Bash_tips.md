```sh
# Check syntax in a script:

bash -n your_script_name
```
-------------------------

```sh
#!/bin/bash
#=====================================
# Change many files Encoding.
#=====================================
# iconv -f ENCODING_FROM -t ENCODING_INTO > NEW_RECODED_FILE
# CONFIG:
TYPES="*.txt";
FROM='cp1251';
TO='utf8';
FILEPrefix='.utf8';

# START:
if [ -z $1 ]
then
        echo "Recoding files: $TYPES from the Dir $FROM into $TO";
        echo "Using path: $0 "; echo;
        exit 1;
else
        echo "Recoding files: $TYPES from $1 path $FROM into $TO";
        echo;
        find $1 -name "$TYPES" -type f -print0|while read -d '' SOURCE; do
                echo "========== Recoding: $SOURCE From: $FROM into: $TO ==========";
                cat "$SOURCE"|iconv -f $FROM -t $TO > "$SOURCE$FILEPrefix";
                wait;
        done;
        exit 0;
fi
# end
```