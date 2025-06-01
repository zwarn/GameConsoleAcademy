using tetris;
using UnityEngine;
using Zenject;

namespace DefaultNamespace
{
    public class GameInstaller : MonoInstaller
    {
        [SerializeField] private TetrisController tetrisController;
        [SerializeField] private PieceGenerator pieceGenerator;
        [SerializeField] private InputController inputController;
        [SerializeField] private TetrisColorRepository colorRepository;
        [SerializeField] private TetrisView tetrisView;
        [SerializeField] private RemainingTilesCount remainingTilesCount;
        [SerializeField] private PiecePreviewSystem piecePreviewSystem;
        [SerializeField] private DropShadow dropShadow;
        [SerializeField] private TetrisLayerVisualizer layerVisualizer;
        [SerializeField] private AudioController audioController;

        public override void InstallBindings()
        {
            Container.Bind<TetrisController>().FromInstance(tetrisController);
            Container.Bind<PieceGenerator>().FromInstance(pieceGenerator);
            Container.Bind<InputController>().FromInstance(inputController);
            Container.Bind<TetrisColorRepository>().FromInstance(colorRepository);
            Container.Bind<TetrisView>().FromInstance(tetrisView);
            Container.Bind<RemainingTilesCount>().FromInstance(remainingTilesCount);
            Container.Bind<PiecePreviewSystem>().FromInstance(piecePreviewSystem);
            Container.Bind<DropShadow>().FromInstance(dropShadow);
            Container.Bind<TetrisLayerVisualizer>().FromInstance(layerVisualizer);
            Container.Bind<AudioController>().FromInstance(audioController);

            Container.Bind<PieceView>().FromComponentInHierarchy().AsTransient();
        }
    }
}