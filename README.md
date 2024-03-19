<h1>Parque de Diversão CRUD em C# .NET 8 com ORM</h1>
<p>Este é um projeto de CRUD (Create, Read, Update, Delete) em C# .NET 8 que gerencia um Parque de Diversão. Ele utiliza um ORM (Object-Relational Mapper) para facilitar a interação com o banco de dados.</p>

<ul>Funcionalidades</ul>
<li>CRUD completo: Capacidade de criar, visualizar, atualizar e excluir registros nas quatro tabelas principais: Setor, Atração, InfoAtração e Barraquinha. </li>
<li>Relacionamentos: O projeto gerencia os relacionamentos entre as tabelas da seguinte forma:</li>
Um Setor pode conter muitas Atrações e muitas Barraquinhas.
Cada Atração está associada a uma única InfoAtração.
Tecnologias Utilizadas
C#: Linguagem de programação principal.
.NET 8: Framework utilizado para desenvolver a aplicação.
ORM (Object-Relational Mapper): Utilizado para mapear objetos de software para tabelas de banco de dados, facilitando a interação com o banco de dados.
Como Executar
Certifique-se de ter o ambiente de desenvolvimento .NET 8 configurado em sua máquina.
Clone este repositório.
Abra o projeto em sua IDE preferida.
Configure a conexão com o banco de dados no arquivo de configuração correspondente.
Execute o projeto.
Estrutura do Projeto
O projeto está estruturado da seguinte forma:

Models: Contém as classes que representam os modelos de dados (Setor, Atração, InfoAtração, Barraquinha).
Repositories: Classes responsáveis pela interação com o banco de dados, incluindo operações CRUD.
Controllers: Controladores da aplicação que gerenciam as requisições HTTP e chamam os métodos apropriados nos repositórios.
Views: Não aplicável para este projeto, já que é um aplicativo de console, mas poderia incluir interfaces de usuário se fosse uma aplicação web ou de desktop.
