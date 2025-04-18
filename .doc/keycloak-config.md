[Voltar para o README](../README.md)

## Configuração do Keycloak

Esta seção garante que a configuração seja feita corretamente para que os processos de autenticação e validação do JWT funcionem adequadamente.

## Criação do Cliente
 - Clique no menu **Clients** e depois clique no botão **Create Client**.
 - Vamos criá-lo com o nome **web-api** e depois clique em **Next**.
 - Na segunda tela, habilite a opção **Client authentication** e clique em **Save**.

## Configurando o cliente nos projetos Api e Consumers
 # Keycloak
 - Clique no menu **Clients**
 - Clique em **web-api**
 - Clique na aba **Credentials**
 - Clique no botão de copiar do **Client Secret**
 
 # Projetos
 - Vá até o arquivo **appsettings.json**
 - Em **Keycloak:credentials:secret**
 - Substitua pelo valor copiado

## Client Scope
 - Clique no menu **Client scopes**, depois clique em **Create Client Scope**.
 - O **scope** deve ser nomeado como **web-api-scope**
 - Defina **Type: Default**
 - Defina **Protocol: OpenId Connect**
 - Defina **Include in token scope: On**
 - Clique em **Save**
 - Clique na aba **Mappers** e clique em **Create a new mapper**
 - Selecione **By configuration**
 - Clique na opção **Audience**
 - Nomeie como **web-api-audience**
 - Liste o cliente **web-api** na opção **Included Client Audience**.
 - Defina **Add to access token: On**
 - Clique em **Save**
 - Clique em **Add Mapper**

## Atributo Personalizado
 - Vá para **Realm Settings** > **User profile**
 - Clique em **Create Attribute**
 - Nomeie como **custom_id**
 - Defina **Display name** como **Custom Id**
 - Defina **Enabled when: Always**
 - Defina **Required field: On**
 - Defina **Required for: Only users**
 - Defina **Required when: always**
 - Defina **Who can edit?: Admin**
 - Defina **Who can view?: Admin**
 - Clique em **Save**
 
## Adicionando atributo personalizado ao token
 - Clique no menu **Client scopes**
 - Clique em **web-api-scope**
 - Clique na aba **Mappers**
 - Clique em **Add mapper** > **By configuration**
 - Selecione **User Attribute**
 - Nomeie como **custom-id**
 - Defina o **User Attribute** como **custom_id**
 - Defina o **Token claim name** como **customId**
 - Defina o **Claim JSON Type** como **String**
 - Defina **Add to ID token: On**
 - Defina **Add to access token: On**
 - Defina **Add to user info: On**
 - Defina **Add to token introspection: On**
 - Clique em **Save**
 
## Adicionando o id do usuário ao token
 - Clique no menu **Client scopes**
 - Clique em **web-api-scope**
 - Clique na aba **Mappers**
 - Clique em **Add mapper** > **By configuration**
 - Selecione **User Property**
 - Nomeie como **user-id**
 - Defina a **Property** como **id**
 - Defina o **Token claim name** como **userId**
 - Defina o **Claim JSON Type** como **String**
 - Defina **Add to ID token: On**
 - Defina **Add to access token: On**
 - Defina **Add to user info: On**
 - Defina **Add to token introspection: On**
 - Clique em **Save**
