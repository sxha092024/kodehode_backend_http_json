{
  description = "C# I/O stream + serialisation";

  inputs = {
    flake-utils.url = "github:numtide/flake-utils";
  };

  outputs = {
    self,
    nixpkgs,
    flake-utils,
  }:
    flake-utils.lib.eachDefaultSystem (
      system: let
        pkgs = nixpkgs.legacyPackages.${system};
        dotnetPkg = with pkgs.dotnetCorePackages;
        # NOTE: the first sdk in the list is the one whose cli utility is present in the environment
          combinePackages [
            sdk_9_0
            sdk_8_0
          ];
      in {
        devShells.default = pkgs.mkShell {
          buildInputs = with pkgs; [];

          nativeBuildInputs = with pkgs; [
            dotnetPkg
            csharp-ls
          ];
          shellHook = ''
            export DOTNET_ROOT="${dotnetPkg}";
            echo ".net root: '${dotnetPkg}'"
            echo ".net version: $(${dotnetPkg}/bin/dotnet --version)"
            echo ".net SDKs:"
            echo "$(${dotnetPkg}/bin/dotnet --list-sdks)"
            echo "Flake shell active..."
          '';
        };
      }
    );
}
