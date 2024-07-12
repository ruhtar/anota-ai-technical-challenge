# To-Do List - Sistema Backend

## Planejamento

- [X] Definir a arquitetura do sistema
  - [X] Escolher framework (e.g., Express, Django, Flask)
  - [X] Definir banco de dados (e.g., PostgreSQL, MongoDB)
  - [X] Definir mensageria PubSub (SQS, RabbitMQ)

## Desenvolvimento


### Banco de Dados

- [X] Configurar conexão com banco de dados

### APIs e Endpoints

- [X] Criar endpoints para CRUD de categoria
- [X] Criar endpoints para CRUD de produto

### Requisitos do Usuário

- [ ] Registrar produto com dono
  - [ ] Criar endpoint para registrar produto (title, description, price, category, owner ID)
  - [ ] Validar dados de entrada
  - [ ] Salvar produto no banco de dados

- [ ] Registrar categoria com dono
  - [ ] Criar endpoint para registrar categoria (title, description, owner ID)
  - [ ] Validar dados de entrada
  - [ ] Salvar categoria no banco de dados

- [ ] Associar produto a uma categoria
  - [ ] Criar endpoint para associar produto a uma categoria
  - [ ] Validar associação (produto só pode estar em uma categoria)

- [ ] Atualizar dados de produto ou categoria
  - [ ] Criar endpoint para atualização de produto
  - [ ] Criar endpoint para atualização de categoria
  - [ ] Validar dados de entrada
  - [ ] Atualizar dados no banco de dados

- [ ] Deletar produto ou categoria do catálogo
  - [ ] Criar endpoint para deletar produto
  - [ ] Criar endpoint para deletar categoria
  - [ ] Validar operação
  - [ ] Remover dados do banco de dados

### Considerações de Escalabilidade e Desempenho

- [ ] Gerenciar múltiplas requisições por segundo
  - [ ] Implementar técnicas de otimização de performance
  - [ ] Considerar caching para reduzir consultas ao banco de dados

- [ ] Compilar catálogo como JSON
  - [ ] Criar função para gerar JSON do catálogo
  - [ ] Armazenar catálogo JSON no banco de dados para acesso rápido

## Integração com AWS

- [ ] Publicar mudanças no catálogo para "catalog-emit" no AWS SQS
  - [ ] Implementar função para publicar mensagem no SQS

- [ ] Implementar consumer para ouvir mudanças no catálogo
  - [ ] Criar consumidor que escuta mensagens do SQS
  - [ ] Buscar catálogo do dono no banco de dados
  - [ ] Gerar JSON do catálogo

- [ ] Publicar catálogo JSON no AWS S3
  - [ ] Implementar função para enviar JSON ao bucket do S3


### Testes

- [ ] Escrever testes unitários
  - [ ] Testes para modelos
  - [ ] Testes para controladores
- [ ] Escrever testes de integração
  - [ ] Testes para endpoints

### Documentação

- [ ] Documentar APIs (e.g., Swagger, Postman)
- [ ] Escrever documentação para desenvolved
