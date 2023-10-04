using Arc4u.Dependency;
using Prism.DI.Ioc;

namespace EG.DemoPCBE99925.ManageCourse.WPF;

class PrismContainerExtension : DIContainerExtension
{
    public PrismContainerExtension(IContainer container) : base(container)
    {
    }

    public override void FinalizeExtension()
    {
        Container.CreateContainer();

        DependencyContext.CreateContext(Container);
    }
}