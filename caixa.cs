using Godot;
using System;
//indica que a caixa é um tipo character body 2d
public partial class caixa : CharacterBody2D
{
	//Uma constante de velocidade.
	public const float SPEED = 300;
	//Uma costante de Gravidade.
	public const float GRAVITY = 1000;
	//Inicialiaza a variavel como null(nula).
	public player_2d player = null;
	//...
	public Vector2 velocity = new Vector2();
	public bool movendoObjeto = false;
	
	
	// Called when the node enters the scene tree for the first time.
	public override void _Ready()
	{
		//Player recebe o node do player_2d o renomeando como "player",
		
		player = GetNode<player_2d>("player");
	}

	// Called every frame. 'delta' is the elapsed time since the previous frame.
	public override void _Process(double delta)
	{
		
		//'V'(é uma variável reservada da godot) diferente de 'v'(variavel criada para xy)
		
		velocity = Velocity;
		// gravidade * delta 
		velocity.Y += GRAVITY * (float) delta;
		// igualando 'v' x=0
		velocity.X = 0;
		// vector 2 recebe direção
		Vector2 direction = Input.GetVector("ui_left", "ui_right","ui_up","ui_down");
		// se o player move o obejto, altera a direção da caixa.
		GD.Print(player.movendoObjeto);
		if (player.movendoObjeto) {
			movendoObjeto = true;
			
			
			// movetoward = se mover até; 1° parâmetro, ponto de partida; 2° direção; 3° velocidade.
			// Mathf é uma classe estática matemática.
			velocity.X = Mathf.MoveToward(Velocity.X, direction.X * SPEED / 5, SPEED/5);
		}
		
		//GD.Print(direction.X);
		//GD.Print(player.Position.X);
		//GD.Print(Position.X);
		//se o player tiver do lado direito da caixa
		
		if (player.Position.X > Position.X) {
			if (direction.X == -1) {
				((AnimationPlayer)GetNode("anim")).Play("empurrar");
			}else{
				((AnimationPlayer)GetNode("anim")).Play("puxar");
			}
			//se o player tiver do lado esquerdo da caixa
		}else{
			if (direction.X == 1) {
				((AnimationPlayer)GetNode("anim")).Play("puxar");
			}else{
				((AnimationPlayer)GetNode("anim")).Play("empurrar");
			}
		}
		Velocity = velocity;
		//faz o personagem se mover.
		MoveAndSlide();
	}
	// detecta o player entrando na área da caixa.
	private void _on_area_contato_body_entered(Node2D body)
	{ // testando se o nome "body" é igual a "player".
		if (body.Name == "player") {
			// se chegar na caixa manda mensagem.
			GD.Print("chegou na caixa, aperta X aí!");
			// criando uma variável e atribuindo um valor.
			player_2d jogador = (player_2d) body;
			//em contato com o objeto a variável fica "true"
			jogador.contatoObjeto = true;
		}
	}
	// detecta quando o player sai área da caixa.
	private void _on_area_contato_body_exited(Node2D body)
	{	// testando se o nome "body" é igual a "player".
		if (body.Name == "player") {
			// se sair da caixa manda mensagem.
			GD.Print("saiu da caixa");
			// criando uma variável e atribuindo um valor.
			player_2d jogador = (player_2d) body;
			//fora de contato com o objeto a variável fica "false"
			jogador.contatoObjeto = false;
			//movendo a caixa "false"
			jogador.movendoObjeto = false;
		}
	}
}
