# Raizen.Net.Publico

Criar uma aplicação C#.NET para Visual Studio 2019 que contém os seguintes pontos:

1. CRUD para Cadastro de cliente pata tabela abaixo.

create table tblCliente
(
    ClienteId      int auto_increment
        primary key,
    Nome           varchar(255) not null,
    Email          varchar(100) not null,
    DataNascimento datetime     not null,
    CEP          varchar(9)  not null,
    Ativo          boolean   not null,
    DataCriacao    datetime     not null,
    DataAlteracao  datetime     not null,
    CriadoPor      int          not null,
    AlteradoPor    int          not null,
);
 
2. O CEP deverá ser consultado através da API de pesquisa de CEP (Exemplo de pesquisa por CEP: viacep.com.br/ws/01001000/json/)
 
3. Persistir os dados em banco de dados (MySQL).

4. Criar camada de integração para conexão com o banco

Host: localhost:3306
Authentication: User & Password
User: root
Password: 
Database: costacrm
URL: jdbc:mariadb://localhost:3306/costacrm

5. Criar camada de persistencia para DAO,  DTO e entidades

6. Criar camada de user interface com as views e controles
 
7. Crira a tela MVC para o CRUD de clientes ativo = true que deverá ser composta de:
	a. Grid com os clientes cadastrados contendo Nome, Email, Datade Nascimento e DataCriacao.
	b. Filtro simples para Nome e Email
	c. Botão no Grid para editar registro (Criar uma tela de edição de cliente)
	d. Botão no grid para excluir registro (Este botão de comando deverá editar o registro para Ativo = false)
	e. Botão Novo  (Criar uma tela de cadastro de cliente onde o CEP deverá ser consultado pela API)
	f. Mensagem de validação de campos obrigatórios no cadastro de novos clientes.
