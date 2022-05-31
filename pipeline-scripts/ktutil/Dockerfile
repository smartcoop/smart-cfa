FROM nixos/nix

RUN nix-channel --add https://nixos.org/channels/nixpkgs-unstable nixpkgs
RUN nix-channel --update
RUN nix-shell -p krb5 --run "which ktutil"

COPY ./ktutil.sh .
CMD ./ktutil.sh