#!/bin/bash
echo "refresh initialized"
echo "the user is : $user"   > /proc/1/fd/1 2>/proc/1/fd/2
kinit $user@UBIK.BE -k -t /app/$user.keytab > /proc/1/fd/1 2>/proc/1/fd/2
klist > /proc/1/fd/1 2>/proc/1/fd/2
kinit -kt /app/$user.keytab $user@UBIK.BE > /proc/1/fd/1 2>/proc/1/fd/2
klist > /proc/1/fd/1 2>/proc/1/fd/2
