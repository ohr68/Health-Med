## 🏗️ Diagrama Lógico da Arquitetura
                         
  ![Imagem do diagrama](https://github.com/ohr68/Health-Med/blob/master/images/diagrama.png?raw=true)


## ✅ Justificativa das Escolhas Técnicas
### 🔄 Hexagonal Architecture (Ports & Adapters)
Separação clara entre core (domínio + aplicação) e infra/drivers.

Facilita testes, substituição de tecnologias, e manutenção.

## 🐳 Docker
Containers para cada API, banco de dados e mensageria.

Facilita o deploy, testes locais consistentes e pipelines de CI/CD.

O docker-compose.yml sugere orquestração local simples e reprodutível.

## 🛡️ Ocelot (HealthMed.ApiGateway)
Gateway de entrada centralizado para as APIs.

Permite autenticação, logging, caching, roteamento e versionamento.

Simplifica o front-end e clientes externos ao consumir APIs.

## 🧠 MediatR (em Application Layer)
Desacoplamento entre a chamada e o manipulador da lógica.

Foco em Command/Query Responsibility Segregation (CQRS).

Permite cross-cutting concerns como logging, validação e transações via pipelines.

## 📨 MassTransit (HealthMed.Messaging + HealthMed.Consumers)
Comunicação assíncrona entre microsserviços.

Suporte a eventos, retries, sagas e dead-letter queues.

Alta escalabilidade e resiliência.

## ✅ FluentValidation
Validação fluente e expressiva para comandos e modelos.

Facilita testes unitários e customizações de regra de negócio.

## 🔐 Keycloak
Servidor de autenticação e autorização baseado em OpenID Connect.

Permite login social, SSO, RBAC e suporte a multi-tenant.

Reduz o código de autenticação customizado e centraliza a gestão de usuários.

Para garantir a sincronização de novos usuários foram criados consumers utilizando o masstransit. Ao cadastrar
um médico/paciente os dados são enviados para seus respectivos consumers onde ocorre o mapeamento das entidades para
a representação de um usuário do keycloak e, utilizando um Saga Pattern, o cadastro é feito nas seguintes etapas:
  - Cadastro do usuário
  - Consulta de roles atreladas ao realm
  - Consulta de roles atreladas ao client
  - Mapeamento dos resultados para a propriedade que representa as roles do usuário
  - Alteração do usuário cadastrado para atribuição das roles necessárias para acesso à api
    

## ⚙️ Como os requisitos não funcionais são atendidos

Requisito	                          Como é atendido
Escalabilidade	                    MassTransit com mensageria desacopla serviços. Containers permitem replicação.
Segurança	                          Keycloak + Ocelot fazem o controle de acesso, tokens JWT e RBAC.
Manutenibilidade	                  Arquitetura hexagonal facilita isolar mudanças em infraestrutura ou domínio.
Testabilidade	                      Projetos separados de testes: Unit, Integration e Functional.
Desempenho	                        Cache em HealthMed.Caching e messaging assíncrona ajudam na performance.
Disponibilidade	                    APIs desacopladas, tolerância a falhas via MassTransit e retries.
Portabilidade	                      Docker garante que o projeto funcione igual em qualquer ambiente.


As documentações com mais detalhes podem ser acessadas em
##Keycloak  
[Configuração do Keycloak](/.doc/keycloak-config.md)

## Instruções para executar o projeto
[Execução do projeto](/.doc/instrucoes-execucao.md)

## Endereços das interfaces dos serviços
[Endereços](/.doc/enderecos-projetos.md)

# Healthmed Kubernetes (Minikube)

Este repositório contém os manifests Kubernetes para rodar o stack Healthmed localmente via Minikube **sem Ingress**, utilizando `NodePort`, `Secrets`, `PVC` e `Kustomize`.

## Pré-requisitos

- [kubectl](https://kubernetes.io/docs/tasks/tools/)
- [minikube](https://minikube.sigs.k8s.io/)
- [kustomize](https://kubectl.docs.kubernetes.io/installation/kustomize/)

## Executar deploy.sh

```sh
./k8s/deploy.sh
```