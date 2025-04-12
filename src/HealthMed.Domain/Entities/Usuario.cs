using HealthMed.Domain.Enums;

namespace HealthMed.Domain.Entities;

public class Usuario : Entidade
{
    public string? Senha { get; set; }
    public bool LoginLiberado { get; private set; }
    public StatusSincronizacaoUsuario Status { get; set; }
    public TipoUsuario TipoUsuario { get; set; }
    
    public virtual ICollection<Paciente>? Pacientes { get; private set; }
    public virtual ICollection<Medico>? Medicos { get; private set; }
    
    public void CadastroSincronizado()
    {
        DefinirAtualizacao();
        LoginLiberado = true;
        Sincronizado();
    }

    public void AtualizarSincronizado()
    {
        DefinirAtualizacao();
        Sincronizado();
    }

    public void DeveSincronizar()
    {
        Status = StatusSincronizacaoUsuario.Pendente;
    }

    private void Sincronizado()
    {
        Status = StatusSincronizacaoUsuario.Concluido;
    }

    public bool PodeLogar() => LoginLiberado;
}