#! /usr/bin/env nix-shell
#! nix-shell -i bash -p krb5

user=$KTUTIL_USER
pass=$KTUTIL_PASS

printf "%b" "addent -password -p $user@UBIK.BE -k 1 -e aes256-cts-hmac-sha1-96\n$pass\nwrite_kt $user.keytab" | ktutil
cp $user.keytab /files
