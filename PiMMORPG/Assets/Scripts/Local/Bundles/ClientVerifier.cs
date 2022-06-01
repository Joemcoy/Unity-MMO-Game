#if UNITY_EDITOR
//#define LOCAL

using UnityEditor;
#endif

using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Linq;

using UnityEngine;
using UnityEngine.SceneManagement;
using uObject = UnityEngine.Object;

using tFramework.Helper;
using tFramework.Data.Manager;
using tFramework.Data.Serializer;

using PiMMORPG;

namespace Scripts.Local.Bundles
{
    using Devdog.InventoryPro;
    using Scripts.Local.Control;
    using System.Text;
    using tFramework.Factories;
    using UI;

    public class ClientVerifier : SingletonBehaviour<ClientVerifier>
    {
        public string URL;
#if UNITY_EDITOR
        public bool Check = false;
#endif

        public void Run()
        {
            StartCoroutine(Checker());
        }

        IEnumerator Checker()
        {
            yield return null;
            var operation = SceneManager.LoadSceneAsync(1);
            while (!operation.isDone)
                yield return null;

            var loading = FindObjectOfType<LoadingScreen>();
            var Loader = BundleLoader.Instance;

            if (!ConfigurationManager.Load(ref Application.Configuration))
            {
                loading.InfoText = "Falha ao carregar as configurações do jogo!";
            }
            else
            {
                loading.InfoText = "Baixando arquivo de verificação...";
                Debug.LogWarning(URL);
                using (var Web = new WWW(URL))
                {
                    while (!Web.isDone)
                        loading.Progress = Web.progress;
                    yield return Web;
                    loading.Progress = 0;

                    byte[] buffer = null;
                    var cp = Path.Combine(Environment.CurrentDirectory, "Checksum.xml");
                    if (string.IsNullOrEmpty(Web.error))
                    {
                        loading.InfoText = "Carregando arquivo de verificação...";

                        var h1 = File.Exists(cp) ? HashHelper.CalculateFileMD5(cp) : string.Empty;
                        var h2 = HashHelper.CalculateMD5(Web.bytes);

                        if (h1 != h2)
                        {
                            if (File.Exists(cp))
                                File.Delete(cp);

                            using (var stream = File.OpenWrite(cp))
                                stream.Write(Web.bytes, 0, Web.bytesDownloaded);
                        }
                        buffer = Web.bytes;
                    }
                    else
                    {
                        Debug.LogError(Web.error);
                        if (File.Exists(cp))
                            buffer = File.ReadAllBytes(cp);
                        else
                        {
                            loading.InfoText = "Falha ao carregar o arquivo de verificação...";
                            yield break;
                        }
                    }
                    buffer = CryptHelper.DecryptRijndael(buffer);

                    using (var Stream = new MemoryStream(buffer))
                    {
                        FileData[] Files = null;
                        if (XMLSerializer.Load(ref Files, Stream))
                        {
                            loading.InfoText = "Verificando arquivos...";
                            int counter = 0;

                            var bundles = Files
                                .Where(f => Path.GetExtension(f.FilePath) != ".hash" && f.FilePath.Split(Path.DirectorySeparatorChar).First() == "Bundles")
                                .Select(f => f.FilePath)
                                .ToArray();
                            loading.Maximum = bundles.Length + 1f;

                            var Dir = BundleLoader.GetTargetPath();
                            var logger = LoggerFactory.GetLogger(this);
                            foreach (var Info in Files)
                            {
                                Info.FilePath = Info.FilePath.Replace('\\', Path.DirectorySeparatorChar)
                                    .Replace('/', Path.DirectorySeparatorChar);
                                loading.InfoText = string.Format("Verificando arquivo {0} de {1}...", ++counter,
                                    Files.Length);
                                loading.Progress = counter / Files.Length;

                                var FilePath = Path.Combine(Dir, Info.FilePath);

#if UNITY_EDITOR
                                if (Check)
                                {
#endif
                                    var HashFile = FilePath + ".hash";

                                    if (!File.Exists(FilePath))
                                    {
                                        loading.InfoText = string.Format("O arquivo {0} não existe!", Path.GetFileName(Info.FilePath));
                                        logger.LogWarning("File {0} hasn't been found!", Info.FilePath);
                                        yield break;
                                    }
                                    else if (new FileInfo(FilePath).Length != Info.Size)
                                    {
                                        loading.InfoText = string.Format("O arquivo {0} não está atualizado!", Info.FilePath);
                                        logger.LogWarning("File {0} has inconsistent size! (Local:{1} Remote:{2})", Info.FilePath, new FileInfo(FilePath).Length, Info.Size);
                                        yield break;
                                    }
                                    else if (Path.GetExtension(FilePath) != ".hash")
                                    {
                                        if (!File.Exists(HashFile))
                                        {
                                            loading.InfoText = string.Format("O arquivo de verificação de {0} não foi encontrado!",
                                                Path.GetFileName(Info.FilePath));
                                            logger.LogWarning("Hash file for {0} hasn't been found!", Info.FilePath);
                                            yield break;
                                        }
                                        else
                                        {
                                            var decoded = CryptHelper.DecryptRijndael(File.ReadAllBytes(HashFile)).Select(b => Convert.ToByte(b >= 5 ? b - 5 : b)).ToArray();
                                            var hash = Encoding.UTF8.GetString(decoded);

                                            if (hash != Info.Hash)
                                            {
                                                loading.InfoText = string.Format("O arquivo {0} não confere na soma!",
                                                Path.GetFileName(Info.FilePath));
                                                logger.LogWarning("File {0} has inconsistent hash! (Local:{1} Remote:{2})", Info.FilePath, hash, Info.Hash);
                                                yield break;
                                            }
                                        }
                                    }
                                    else if (Path.GetExtension(FilePath) == ".hash")
                                    {
                                        if (!File.Exists(FilePath))
                                        {
                                            loading.InfoText = string.Format("O arquivo {0} não está atualizado!", Path.GetFileName(Info.FilePath));
                                            logger.LogWarning("Hash file for {0} doesn't exists!", Info.FilePath);
                                            yield break;
                                        }
                                        else if (HashHelper.CalculateFileMD5(FilePath) != Info.Hash)
                                        {
                                            loading.InfoText = string.Format("O arquivo {0} não está atualizado!", Path.GetFileName(Info.FilePath));
                                            logger.LogWarning("Hash file {0} doesn't match to remote! (Local:{1} Remote:{2})", Info.FilePath, HashHelper.CalculateFileMD5(FilePath), Info.Hash);
                                            yield break;
                                        }
                                    }
#if UNITY_EDITOR
                                }
#endif
                            }

                            counter = 1;
                            loading.Progress = 0;

                            yield return null;
                            foreach (var bundle in bundles)
                            {
                                loading.InfoText = string.Format("Carregando {0} de {1} arquivos...", counter++, bundles.Length);
#if !LOCAL
                                loading.Progress = counter;
                                var path = Path.Combine(Dir, bundle);
                                var request = AssetBundle.LoadFromFileAsync(path);
                                request.allowSceneActivation = false;
                                while (!request.isDone && request.progress < 0.9f)
                                {
                                    loading.Progress = counter + request.progress;
                                    yield return null;
                                }

                                request.allowSceneActivation = true;
                                while (!request.isDone)
                                {
                                    loading.Progress = request.progress;
                                    yield return null;
                                }
                                loading.Progress = 1f;
                                if (BundleLoader.RegisterBundle(bundle, request.assetBundle))
                                {
#else
                                    Loading.Maximum = 1f;
                                    Loading.Progress = 0;

                                    var rp = bundle.Replace("Bundles", string.Empty).Substring(1).Replace('\\', '/').ToLower();
                                    var paths = AssetDatabase.GetAssetPathsFromAssetBundle(rp);
                                    var asset = paths.Length > 0 ? AssetDatabase.LoadAssetAtPath<uObject>(paths[0]) : null;

                                    if (asset != null)
                                    {
                                        if (asset is SceneAsset)
                                        {
                                            var path = Path.Combine(Dir, bundle);
                                            BundleLoader.RegisterScene("scenes/" + Path.GetFileName(path), path);
                                        }
                                        else
                                        {
                                            yield return StartCoroutine(BundleLoader.RegisterAsset(rp, asset));
                                        }
#endif
                                    yield return null;
                                }
                                else
                                {
                                    loading.InfoText = string.Format("Falha ao carregar o arquivo {0}!", Path.GetFileName(bundle));
                                    yield break;
                                }
                            }

                            loading.InfoText = "Verificação encerrada! Carregando inventário...";
                            var inventory = Instantiate(BundleLoader.LoadPrefab("prefabs/inventory"));
                            var manager = inventory.GetComponentInChildren<ItemManager>(true);
                            DontDestroyOnLoad(inventory);
                            yield return null;

                            loading.InfoText = "Carregando definições de inventário...";

                            var db = manager.sceneItemDatabase;
                            db.items = BundleLoader.LoadAssetsOf<InventoryItemBase>("inventory/items/").OrderBy(i => i.ID).ToArray();
                            db.statDefinitions = BundleLoader.LoadAssetsOf<StatDefinition>("inventory/stats/");
                            db.categories = BundleLoader.LoadAssetsOf<ItemCategory>("inventory/categories/");
                            db.currencies = BundleLoader.LoadAssetsOf<CurrencyDefinition>("inventory/currencies/");
                            db.equipmentTypes = BundleLoader.LoadAssetsOf<EquipmentType>("inventory/equiptypes/");

                            foreach (var item in db.items.OfType<EquippableInventoryItem>())
                            {
                                item.equipmentType = db.equipmentTypes.First(e => e.name == item.equipmentType.name);
                                item.category = db.categories.First(c => c.name == item.category.name);
                                foreach (var decorator in item.stats)
                                    decorator.stat = db.statDefinitions.First(s => s.name == decorator.stat.name);
                            }
                            yield return null;

                            loading.InfoText = "Carregando definições de equipamentos...";
                            var cUI = inventory.GetComponentInChildren<CharacterUI>(true);
                            foreach (var eslot in cUI.GetComponentsInChildren<EquippableSlot>(true))
                                if (eslot.equipmentTypes.Length > 0)
                                    eslot.equipmentTypes = db.equipmentTypes.Where(e => eslot.equipmentTypes.Any(et => et.name == e.name)).ToArray();
                            inventory.SetActive(true);
                            yield return null;

                            loading.InfoText = "Carregando menu...";
                            BundleLoader.LoadScene("menu");
                        }
                        else
                            loading.InfoText = "Falha ao carregar a arquivo de verificação!";
                    }
                }

                yield return null;
            }
        }
    }
}