[Voltar para o README](../README.md)

## Como Executar

### Ferramentas necessárias
O Docker Desktop é necessário apenas para verificar ou inspecionar o contêiner.
[Link para instalação do Docker Desktop no Windows](https://docs.docker.com/desktop/setup/install/windows-install/)

### Visual Studio
Clique com o botão direito no projeto **docker-compose** > **Set as StartUp project**. 
Em seguida, clique em **Run**.

### Rider 
Clique com o botão esquerdo no projeto **docker-compose** e expanda-o.
Depois, clique com o botão direito no arquivo **docker-compose.yml** e selecione **Run 'docker-compose.yml'**.

Após seguir os passos acima, o contêiner deve estar rodando com todas as imagens já configuradas corretamente.
Toda a comunicação entre a WebApi, o banco de dados e o RabbitMq já está configurada para facilitar o processo de testes.
Agora o projeto está pronto para ser testado.
