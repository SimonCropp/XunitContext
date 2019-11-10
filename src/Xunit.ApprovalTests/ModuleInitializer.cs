using ApprovalTests;

static class ModuleInitializer
{
    public static void Initialize()
    {
        Approvals.RegisterDefaultNamerCreation(() => Namer.Instance);
    }
}