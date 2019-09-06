using ApprovalTests;

static class ModuleInitializer
{
    public static void Initialize()
    {
        var namer = new Namer();
        Approvals.RegisterDefaultNamerCreation(() => namer);
    }
}