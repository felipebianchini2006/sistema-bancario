# Sistema Bancário Educacional

Este projeto nasceu como uma iniciativa acadêmica para explorar conceitos de **engenharia de software**, **arquitetura em múltiplas camadas** e integração de diferentes linguagens no contexto de um sistema bancário.  

A ideia inicial era desenvolver uma plataforma completa que abrangesse:

- Autenticação e gerenciamento de usuários  
- Contas bancárias e operações de crédito/débito  
- Histórico de transações e relatórios  
- Integração com um motor de risco em **C++**  
- Exploração futura de módulos auxiliares em **Python**, **COBOL** e **Java** para simular ambientes corporativos heterogêneos  

---

## 🚀 Tecnologias Utilizadas

- **Back-End:** ASP.NET Core 8 (C#)  
- **Banco de Dados:** PostgreSQL + Entity Framework Core  
- **Testes Automatizados:** xUnit  
- **Integração Nativa Planejada:** C++ (RiskEngine)  
- **Linguagens Consideradas:** Python, Java, COBOL (não implementadas)  

---

## 📂 Estrutura (na fase ativa do projeto)

```
BankApi/
 ┣ Controllers/        → Endpoints da API
 ┣ Data/               → Contexto do banco e Migrations
 ┣ Dto/                → Objetos de transferência de dados
 ┣ Models/             → Entidades do domínio
 ┣ Services/           → Regras de negócio
 ┣ Tests/              → Testes automatizados
 ┗ Program.cs          → Ponto de entrada da aplicação
```

---

## 📜 Status do Projeto

Após análise, decidi **interromper o desenvolvimento** e **arquivar o repositório**.  
O código disponível cobre até o módulo de **Histórico de Transações**, mas não foram concluídos os módulos de empréstimos, motor de risco ou integrações externas.  

O repositório permanece público como referência acadêmica e registro do aprendizado.  

---

## 📖 Licença

Uso educacional e de estudo.  
