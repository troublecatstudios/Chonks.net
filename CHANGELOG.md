# Changelog

<a name="0.4.0"></a>
## 0.4.0 (2021-08-30)

### Added

- ✨ save files can be deleted [[ca5f5b9](https://github.com/troublecatstudios/Chonks.net/commit/ca5f5b9ee1545500ca1113e997eb5184797eb68d)]
- ✨ chunk data can be overwritten now [[1d4df58](https://github.com/troublecatstudios/Chonks.net/commit/1d4df5818cd5c372f32021fb732b0e4d8ca6b6da)]


<a name="0.3.0"></a>
## 0.3.0 (2021-08-28)

### Fixed

- 🐛 Registry&lt;T&gt; now prunes null items before returning its list [[de52c76](https://github.com/troublecatstudios/Chonks.net/commit/de52c760089537652b34846f4a174d471d7d2145)]


<a name="0.2.0"></a>
## 0.2.0 (2021-08-27)

### Added

- ✨ SaveStores can be notified when all chunks are loaded [[0ebc45e](https://github.com/troublecatstudios/Chonks.net/commit/0ebc45e2715c9f16672263e1e512596f1dba3782)]
- 📈 ChunkDataSegment no longer re-serializes during ToJson() [[9f49ab3](https://github.com/troublecatstudios/Chonks.net/commit/9f49ab326a03d6efeda2f85b865b13f86e698cc2)]

### Miscellaneous

- 📄 adding docs on how to create a new release [[c517b0d](https://github.com/troublecatstudios/Chonks.net/commit/c517b0d806cbd37aa7e1f3e170ff0d4d372b3d0e)]
- 🧹 adds some of the housekeeping files to the solution [[296e924](https://github.com/troublecatstudios/Chonks.net/commit/296e92463c04a1c2693a065b83ca26d541206bd4)]
- 🧹 adds docs to solution [[d24afab](https://github.com/troublecatstudios/Chonks.net/commit/d24afabe541f888a831a5ba8025c3db5dd1d47c3)]
- 📄 doc comment updates [[bbc56a6](https://github.com/troublecatstudios/Chonks.net/commit/bbc56a67499be962948e5012d33e2635e52ae5d9)]


<a name="0.1.0"></a>
## 0.1.0 (2021-08-24)

### Added

- 👷‍♂️ ok, forget this dateformat stuff [[ce581a9](https://github.com/troublecatstudios/Chonks.net/commit/ce581a9b91ac97d55aa60cfa09e955d3a6bca495)]
- 👷‍♂️ I&#x27;m an idiot [[8a499a4](https://github.com/troublecatstudios/Chonks.net/commit/8a499a4b3fd64b66615a14ee41f98d4240e83b33)]
- 👷‍♂️ hopefully fixing the last action run failure [[c233056](https://github.com/troublecatstudios/Chonks.net/commit/c2330568337e69e009bd4b192bbaddb4f6905610)]
- ✅ wrapping up tests for Chonks [[39577a6](https://github.com/troublecatstudios/Chonks.net/commit/39577a60ba2df598f9b3da7133e2c7c71f7d50d9)]
- ✨ adds example unity project [[d0e3662](https://github.com/troublecatstudios/Chonks.net/commit/d0e36621c4b32a5de76912affd6c25f8ffb9d7e0)]
- ✅ adds tests for supporting custom JSON converters [[89506ef](https://github.com/troublecatstudios/Chonks.net/commit/89506ef36ea8b0870f149efbf6bfec1171777c97)]
- 🎉 initial commit for chonks code [[d049075](https://github.com/troublecatstudios/Chonks.net/commit/d049075cb31c5d55df85d5ece3f8db37b4b7cdfe)]

### Changed

- ♻️ updates Chonksy unity project with code changes from Chonks [[9251b5e](https://github.com/troublecatstudios/Chonks.net/commit/9251b5e5c71b10d9af539f0c8aedd02880da19dc)]
- ♻️ updates code for Chonks.Unity to work with new API changes [[56d0758](https://github.com/troublecatstudios/Chonks.net/commit/56d07580eb83c665c34b2c21eaf4be3d6da1ada7)]
- ♻️ removes SaveController in favor of separate registries [[2458988](https://github.com/troublecatstudios/Chonks.net/commit/2458988d1047def05827fd9b26b2ff0a8ce6c2d1)]
- ♻️ refactor Interpreter to allow chunk modifications during save [[d275b9c](https://github.com/troublecatstudios/Chonks.net/commit/d275b9c320b7fc5ee515fa76c9fad0e97a2b5a07)]
- ♻️ target netstandard2.0 for testing project [[a67fd55](https://github.com/troublecatstudios/Chonks.net/commit/a67fd553800b9243e1981710809c21b1e3f363fb)]
- ♻️ change target framework to netstandard2.0 [[f659f69](https://github.com/troublecatstudios/Chonks.net/commit/f659f69149dc6e0ae452dfdbed51b23bc34a945f)]

### Fixed

- 🐛 fix syntax error in Chonksy example [[9dd299f](https://github.com/troublecatstudios/Chonks.net/commit/9dd299fcad39654affe12dd72ab4628c6f9ac805)]
- 🐛 fixes casing for packages dir [[af13876](https://github.com/troublecatstudios/Chonks.net/commit/af13876febe03b20897c3e2e62dce1d1d9d2ad96)]

### Miscellaneous

-  :shipit: update Chonks.dll for Chonks.Unity [[8b558f0](https://github.com/troublecatstudios/Chonks.net/commit/8b558f0adcd4c85bdb77740c27e5440c9ad24378)]
- 🗑️ this release thing is more trouble than it&#x27;s worth. bining it for now. [[9b14b87](https://github.com/troublecatstudios/Chonks.net/commit/9b14b879c7fecd55ae61eccd3aafd7df8b25ada6)]
-  Merge pull request [#3](https://github.com/troublecatstudios/Chonks.net/issues/3) from troublecatstudios/add-continuous-integration [[5ba0ed0](https://github.com/troublecatstudios/Chonks.net/commit/5ba0ed010d369ef8c9d1ccc969f27cd5a85b6aeb)]
-  👷allow release action to be run manually [[2bbeeaa](https://github.com/troublecatstudios/Chonks.net/commit/2bbeeaa4c2d85d971b58d9b6c8e134f48f3b5007)]
-  👷run release action when release is created [[ebc75f5](https://github.com/troublecatstudios/Chonks.net/commit/ebc75f507aa90d7e639a8050997a330db6e75729)]
-  :shipit: v0.1.0 (Chonks.Unity) [[f508f80](https://github.com/troublecatstudios/Chonks.net/commit/f508f8074485e137a212a21966edc0e9271b0b93)]
-  v0.1.0 [[577fa4c](https://github.com/troublecatstudios/Chonks.net/commit/577fa4cf94ef666b007d7a3f48f43e53f6717d9b)]
- 📝 updates readme documentation [[4ea407d](https://github.com/troublecatstudios/Chonks.net/commit/4ea407d5e28f64e02f08cae1069d50282b05fe15)]
-  update Chonks.dll for Chonks.Unity [[157d342](https://github.com/troublecatstudios/Chonks.net/commit/157d3429b4e76e9d6348593b9307d6e9e97c6104)]
-  :shipit: v0.0.3 of Chonks.Unity [[622db04](https://github.com/troublecatstudios/Chonks.net/commit/622db049ffd23293d96518aa93007f07ac640efc)]
- 🧹 unignore more meta files. [[7d8c223](https://github.com/troublecatstudios/Chonks.net/commit/7d8c223145e39c86e18dbda1f8948c89d9a040a7)]
- 🗑️ delete this unused Chonks assembly [[6ace6d6](https://github.com/troublecatstudios/Chonks.net/commit/6ace6d606afcd3ecc779a28ba219a51c6154c415)]
-  :shipit: newer version of Chonks.Unity [[e07d32b](https://github.com/troublecatstudios/Chonks.net/commit/e07d32bc8168ab401344cffdccede69dbe6a8a2b)]
-  Merge branch &#x27;develop&#x27; of https://github.com/troublecatstudios/Chonks.net into develop [[a1d9301](https://github.com/troublecatstudios/Chonks.net/commit/a1d93010568df05b5cac9fb9bbae91c738534fba)]
-  adds meta files for Chonks.Unity [[d798a43](https://github.com/troublecatstudios/Chonks.net/commit/d798a43f99aaff2f398dd512e5e77afedb49e5cf)]
-  only run when src/* or tests/* changes [[24204e6](https://github.com/troublecatstudios/Chonks.net/commit/24204e6fc4d7bc9b2f783a56e4ebf447f41c7414)]
-  Update release-unity.yaml [[3453e70](https://github.com/troublecatstudios/Chonks.net/commit/3453e70e1c967aa359d122f60040af2a004e5564)]
-  :octocat: makes the commits for releases [[cc0bc86](https://github.com/troublecatstudios/Chonks.net/commit/cc0bc86970dda4a8c9591f5d9bf3e013237f4b27)]
-  build just builds for tests [[187f647](https://github.com/troublecatstudios/Chonks.net/commit/187f64747f6797ca8af4e710248faed83fe72475)]
-  Create release-unity.yaml [[07c29ba](https://github.com/troublecatstudios/Chonks.net/commit/07c29ba5268061e4ca3d1fc05c4937f8d13a615b)]
-  run builds on develop branch [[6163a0e](https://github.com/troublecatstudios/Chonks.net/commit/6163a0e8b3a8e295232114c4df31d0464147d031)]
-  adds steps to push DLL changes to Chonks.Unity [[05ee39e](https://github.com/troublecatstudios/Chonks.net/commit/05ee39ef48a7885cf089d83ae24d3da39a251520)]
-  Merge pull request [#2](https://github.com/troublecatstudios/Chonks.net/issues/2) from troublecatstudios/add-unity-example [[8b15038](https://github.com/troublecatstudios/Chonks.net/commit/8b15038331299bbdf69ddbeb8d242233cf50f63b)]
-  Merge pull request [#1](https://github.com/troublecatstudios/Chonks.net/issues/1) from troublecatstudios/initial-release [[6c02ae4](https://github.com/troublecatstudios/Chonks.net/commit/6c02ae42b84e982c8bbd3b3cf68cf49cf4e44e80)]
- 🚧 working on adding in a CI process [[46b0422](https://github.com/troublecatstudios/Chonks.net/commit/46b04223e25c4a8c0c6fd5e02839624b18bbeb22)]
- 🧹 cleanup unused methods from ChunkDataSegment [[3a2b162](https://github.com/troublecatstudios/Chonks.net/commit/3a2b162085599230ff8a859d49cd0e5beea89b36)]
-  Merge branch &#x27;initial-release&#x27; of https://github.com/troublecatstudios/Chonks.net into initial-release [[4f31dee](https://github.com/troublecatstudios/Chonks.net/commit/4f31dee0be1cb1bb2dc8cd456fa18ec0adca5b0c)]
-  Create build.yaml [[e42426a](https://github.com/troublecatstudios/Chonks.net/commit/e42426a360da86d09d2e8781cab13510f095c33a)]
- ⚗️ adding more tests [[6e7bd08](https://github.com/troublecatstudios/Chonks.net/commit/6e7bd084f8981b5b852414e60e1c257b510e6ad6)]
-  Initial commit [[f7e9886](https://github.com/troublecatstudios/Chonks.net/commit/f7e988624104d2aa035416742f6ce1b082f46d76)]


