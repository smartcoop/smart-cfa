#! /usr/bin/env bash
kinit $user@UBIK.BE -k -t /app/$user.keytab
declare -p | grep -Ev 'BASHOPTS|BASH_VERSINFO|EUID|PPID|SHELLOPTS|UID' > /container.env
service cron start
dotnet Smart.FA.Catalog.Web.dll
