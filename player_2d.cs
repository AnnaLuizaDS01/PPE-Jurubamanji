using Godot;
using System;
//indica que o player_2d é um tipo character body 2d
public partial class player_2d : CharacterBody2D
{
	//constante da velocidade
	public const float Speed = 300.0f;
	// constante da altura que ele pula
	public const float JumpVelocity = -600.0f;

	// Get the gravity from the project settings to be synced with RigidBody nodes.
	public float gravity = ProjectSettings.GetSetting("physics/2d/default_gravity").AsSingle();

	// hit e gameover
	//cria um vetor de duas posições para a velocidade
	public Vector2 velocity = new Vector2();
	//declara que não está pulando
	public bool isJumping = false;
	//declara que não está caindo
	public bool wasFalling = false;
	//declara que não está levando dano
	public bool isHit = false;
	//declara o número de vidas
	public int hearts = 3;
	
	
	//interação com objetos
	//declara que não está em contato com o objeto
	public bool contatoObjeto = false;
	//declara que não está movendo o objeto
	public bool movendoObjeto = false;
	
	//processo de física-
	public override void _PhysicsProcess(double delta)
	{
		//'V'(é uma variável reservada da godot) diferente de 'v'(variavel criada para xy)
		velocity = Velocity;

		// Add the gravity.
		//Se não estiver no chão, a velocida y + GRAVIDADE *delta
		//o float está convertendo
		if (!IsOnFloor()) {
			velocity.Y += gravity * (float)delta;
		}
		//direções do personagem
		Vector2 direction = Input.GetVector("ui_left", "ui_right", "ui_up", "ui_down");
		//se estiver com vida, não estiver movendo o objeto
		if (hearts > 0 && !movendoObjeto){
			//Se não está pulando e nem tomando dano
			if (!isJumping && !isHit) {
				//está caindo
				if (wasFalling) {
					//chama a animação landing
					((AnimationPlayer)GetNode("anim")).Play("land");
				} else {
					//senão, se a direção é diferente do vector.2zero
					if (direction != Vector2.Zero) {
						//chama a direção de correr
						((AnimationPlayer)GetNode("anim")).Play("run");
					} else {
						//senão, chama a animação idle
						((AnimationPlayer)GetNode("anim")).Play("idle");
					}
				}
				//se não está no chão
				if (!IsOnFloor()) {
					//chama a animação da queda
					((AnimationPlayer)GetNode("anim")).Play("jump2");
					//queda se torna verdadeira
					wasFalling = true;
				}
			} else {
				//a variávbel está caindo recebe "true"
				wasFalling = true;
			}
			//se a direção é diferente do vetor2.zero e não está levando dano
			if (direction != Vector2.Zero && !isHit) {
				
				//muda a direção ao andar
				((Sprite2D)GetNode("sprite2d")).FlipH = direction.X>0;
				// movetoward = se mover até; 1° parâmetro, ponto de partida; 2° direção; 3° velocidade.
				// Mathf é uma classe estática matemática.
				velocity.X = Mathf.MoveToward(Velocity.X, direction.X * Speed, Speed/5);
			} else {
				//se está no chão
				if (IsOnFloor()) {
					// movetoward = se mover até; 1° parâmetro, ponto de partida; 2° direção; 3° velocidade.
					// Mathf é uma classe estática matemática.
					velocity.X = Mathf.MoveToward(Velocity.X, 0, Speed/20);
				} else {
					//senão, altera a velocidade para deslizar
					velocity.X = Mathf.MoveToward(Velocity.X, 0, Speed/200);
				}
			}
			//se está no chão
			if (IsOnFloor()) {
				//não está pulando
				isJumping = false;
			}
			// Handle Jump.
			//se a tecla "espaço" está pressionada e não está pulando e nem toamndo dano
			if (Input.IsActionJustPressed("ui_accept") && !isJumping && !isHit)
			//chama a função jump
				jump();
				//'V'(é uma variável reservada da godot) diferente de 'v'(variavel criada para xy)
			Velocity = velocity;
			//faz o personagem se mover.
			MoveAndSlide();
		} else {
			//senão, direção diferente do vetor
			if (direction != Vector2.Zero) {
				//altera a velocidade, dividindo por 5
				velocity.X = Mathf.MoveToward(Velocity.X, direction.X * Speed / 5, Speed/5);
			} else {
				//senão, velocidade é igual a 0
				velocity.X = 0;
			}
			//'V'(é uma variável reservada da godot) diferente de 'v'(variavel criada para xy)
			Velocity = velocity;
			//faz o personagem se mover.
			MoveAndSlide();
		}
		

		//"moverObjeto" é uma ação mapeada na tecla X. Projeto > Configuração de projeto > MApa Entrada.
		//A variável contatoObjeto é modificada no script da caixa quando o jogador chega perto
		if (Input.IsActionJustPressed("moverObjeto") && contatoObjeto) {
			//se não está movendo o objeto
			if (!movendoObjeto) {
				//se torna verdadeira
				movendoObjeto = true;
				//chama a animação de empurrar
				((AnimationPlayer)GetNode("anim")).Play("empurrar");
			} else {
				//senão, se torna falso
				movendoObjeto = false;
				//chama a animação idle
				((AnimationPlayer)GetNode("anim")).Play("idle");
			}
		}
	}
	//É uma função; "force" está sendo declarada.
	public void hit(int force) {
		//Tomando hit fica "true"
		isHit = true;
		//velocity.x recebe "force"
		velocity.X = force;
		//velocity.y recebe -200
		velocity.Y = -200;
		////'V'(é uma variável reservada da godot) diferente de 'v'(variavel criada para x e y)
		Velocity = velocity;
		//Coração recebe -1.
		hearts -= 1;
		//Escreve a quantidade de corações
		GD.Print(hearts);
		//se quantidade de corações > 0, ele chama animação hit; senão chama die.
		if (hearts > 0) {
			((AnimationPlayer)GetNode("anim")).Play("hit");
		} else {
			((AnimationPlayer)GetNode("anim")).Play("die");
		}
	}
	// função de pular
	public void jump() {
		// chama anim de pular
		((AnimationPlayer)GetNode("anim")).Play("jump");
		// a variavel jump fica verdadeira
		isJumping = true;
		// velocity.y recebe a constante jumpvelocity
		velocity.Y = JumpVelocity;
	}
	//função para cair
	public void jump2() {
		// chama animação de cair
		((AnimationPlayer)GetNode("anim")).Play("jump2");
	}
	// função de acabar uma animação
	private void _on_anim_animation_finished(StringName anim_name) {
		// acaba a anim jump
		switch (anim_name) {
			case "jump":
				jump2();
				break;
			// acaba a anim aterrissar
			case "land":
				wasFalling = false;
				break;
			//acaba a anim de dano
			case "hit":
				isHit = false;
				break;
		}
	}
}
