namespace StegoSystem.Constraints
{
    /// <summary>
    /// Represents recommended constraints for concreat stegosystem or stegomethod;
    /// </summary>
    /// <typeparam name="TContainer"></typeparam>
    /// <typeparam name="TStegoContainer"></typeparam>
    /// <typeparam name="TSecret"></typeparam>
    public interface StegoConstraints<out TContainer, out TStegoContainer, out TSecret> 
        where TContainer : FileTypeConstraints
        where TStegoContainer : FileTypeConstraints
        where TSecret : FileTypeConstraints
    {
        TContainer ContainerFileConstraints { get; }
        TStegoContainer StegoContainerFileConstraints { get; }
        TSecret SecretFileConstraints { get; }
    }
}
