Explicação dos Testes:
Testes de criação (Create):

Valida se o serviço de notificações é chamado quando o ano, o modelo ou a placa são inválidos.

Verifica se a moto não é criada quando a placa já existe no repositório.

Verifica se a moto é criada quando o request é válido.

Testes de atualização (Update):

Valida se o serviço de notificações é chamado para id inválido ou placa inválida.

Verifica se o repositório é chamado para atualizar a moto quando a placa é válida.

Verifica se uma notificação é gerada caso a moto não seja encontrada.

Testes de exclusão (Delete):

Verifica se o serviço de notificações é chamado quando o id fornecido é inválido.

Verifica se o repositório é chamado para deletar a moto quando o id é válido.