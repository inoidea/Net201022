using UnityEngine;
using Zenject;

public class UIInstaller : MonoInstaller
{
    [SerializeField] private Transform _containerForUI;
    [SerializeField] private GameObject _skillsView;

    public override void InstallBindings()
    {
        //GameObject skillViewInstance = Instantiate(_skillsView, _containerForUI);
        //Container.Bind<SkillsView>().FromInstance(skillViewInstance.GetComponent<SkillsView>()).AsCached();

        //Container.Bind<SkillsPresenter>().FromInstance(new SkillsPresenter()).AsCached();
    }
}
