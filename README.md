# ControleCarbono

Aplicação para mapeamento de emissões de carbono em empresas, desenvolvida com .NET. Esta API permite o cadastro de empresas, usuários e registros de emissão de carbono, facilitando o controle e a análise de dados ambientais.

## 📚 Documentação da API - ControleCarbono

Esta documentação descreve os endpoints disponíveis no ambiente local da aplicação **ControleCarbono**, incluindo autenticação e operações com as entidades **Usuário**, **Empresa** e **Emissão de Carbono**.

> **Base URL:** `http://localhost:8080`

---

## 👤 Usuário

### Registro de novo usuário

**POST** `/api/usuario/register`

**Payload:**
```json
{
  "nome": "João Silva",
  "email": "joao@email.com",
  "username": "joao",
  "password": "Senha123@"
}
```

---

## 🔐 Autenticação

### Login do Usuário

**POST** `/api/auth/login`

**Request Body:**
```json
{
  "username": "joao",
  "password": "Senha123@"
}
```

**Response:**
```json
{
  "token": "<JWT Token>"
}
```

> Use o token no cabeçalho de requisições autenticadas:  
> `Authorization: Bearer <JWT Token>`

---


## 🏢 Empresa

### Criar empresa

**POST** `/api/Empresa`

**Headers:**
```
Authorization: Bearer <JWT>
Content-Type: application/json
```

**Payload:**
```json
{
  "nome": "Empresa Teste",
  "cnpj": "12345678000199",
  "username": "empresa_teste",
  "password": "senha123"
}
```

### Listar empresas

**GET** `/api/Empresa`

**Headers:**
```
Authorization: Bearer <JWT>
```

### Buscar empresa por ID

**GET** `/api/Empresa/1`

### Atualizar empresa

**PUT** `/api/Empresa/1`

**Payload:**
```json
{
  "nome": "Empresa Teste Atualizada",
  "cnpj": "12345678000199",
  "username": "empresa_teste_novo",
  "password": "novasenha123"
}
```

### Deletar empresa

**DELETE** `/api/Empresa/1`

---

## 🌱 Emissão de Carbono

### Criar nova emissão

**POST** `/api/emissaocarbono`

**Payload:**
```json
{
  "fonte": "Transporte de Cargas",
  "quantidadeToneladas": 15.75,
  "data": "2023-11-15",
  "descricao": "Emissões do transporte de mercadorias no mês de novembro",
  "empresaId": 1
}
```

### Listar emissões

**GET** `/api/emissaocarbono`

### Buscar emissão por ID

**GET** `/api/emissaocarbono/1`

### Buscar emissões por empresa

**GET** `/api/emissaocarbono/empresa/1`

### Atualizar emissão

**PUT** `/api/emissaocarbono/1`

**Payload:**
```json
{
  "fonte": "Transporte de Cargas (Atualizado)",
  "quantidadeToneladas": 18.20,
  "data": "2023-11-15",
  "descricao": "Emissões atualizadas do transporte de mercadorias",
  "empresaId": 1
}
```

### Deletar emissão

**DELETE** `/api/emissaocarbono/1`

---

## ✅ Observações

- Todos os endpoints devem ser acessados via `http://localhost:8080`;
- É obrigatório o uso do token JWT no cabeçalho `Authorization` para operações autenticadas;
- Requisições sem autenticação ou com token inválido retornarão `401 Unauthorized`.

---

## 🚀 Como rodar a aplicação

1. Clone o projeto:
```bash
git clone https://github.com/mateusC2000/ControleCarbono.git
```

2. Acesse a pasta `Fiap.Api.ControleCarbono`;
```bash
cd Fiap.Api.ControleCarbono/
```

3. Suba a aplicação:

```bash
docker compose up
```

---

### 🧪 Execução ds testes:

1. Após acessar a pasta `Fiap.Api.ControleCarbono`:

Execute o comando:

```bash
docker compose up tests
```

---
