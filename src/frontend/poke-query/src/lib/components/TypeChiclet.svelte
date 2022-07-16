<script>
  export let typeName;

  let typeColors = {
    grass: "#78C850",
    fire: "#F08030",
    water: "#6890F0",
    bug: "#A8B820",
    normal: "#A8A878",
    flying: "#a396e0",
    poison: "#A040A0",
    electric: "#F8D030",
    ground: "#E0C068",
    fairy: "#EE99AC",
    fighting: "#C03028",
    psychic: "#F85888",
    rock: "#B8A038",
    ghost: "#705898",
    ice: "#98D8D8",
    dragon: "#7038F8",
    steel: "#919191",
    dark: "#2E291B",
  };

  const getFontColor = () => {
    const blackTypes = ["electric", "fairy", "ice"];

    if (blackTypes.includes(typeName)) return "black";

    return "white";
  };
</script>

<div
  class="card-text m-2 pt-1 ps-3 pe-3 text-capitalize chiclet"
  style="background-color:{typeColors[typeName]};color:{getFontColor()}"
>
  <p>{typeName}</p>
</div>

<style>
  .chiclet {
    border-radius: 15px;
    font-size: small;
  }
</style>
