using Godot;
using System;

public partial class inimigo_2d : CharacterBody2D
{
	// Called when the node enters the scene tree for the first time.
	public override void _Ready()
	{
	}

	// Called every frame. 'delta' is the elapsed time since the previous frame.
	public override void _Process(double delta)
	{
	}
	// detecta o player tocando o inimigo.
	private void _on_corpo_2d_body_entered(Node2D body)
	{	// testando se o nome "body" é igual a "player".
		if (body.Name == "player") {
			// se tocar no inimigo manda mensagem.
			GD.Print("colidiu com personagem");
			// criando uma variável e atribuindo um valor.
			player_2d jogador = (player_2d) body;
			
			//quando o jogador toma um hit ele recua pra trás
			if (body.Position.X < Position.X) {
				jogador.hit(-300);
			} else {
				jogador.hit(300);
			}
		}
	}
}
