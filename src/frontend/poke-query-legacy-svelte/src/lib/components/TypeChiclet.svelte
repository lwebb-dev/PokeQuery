<script lang="ts">
  export let typeName: string;
  export let isStatic: boolean;
  export let isSmall: boolean = false;

  const modalName: string = `typeModal-${typeName}`;

  const typeColors = {
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
    dark: "#4d4646",
  };

  const blackTypes = [
    "electric", 
    "fairy", 
    "ice", 
    "ground"
  ];

  let typeColor: string = typeColors[typeName];

  const getSmallChicletClass = () => {
    return isSmall ? "chiclet-sm" : "";
  }
  const getFontColor = () => {
    return (blackTypes.includes(typeName)) ? "black" : "white";
  };

  const lightenDarkenColor = (col, amt) => {
    var usePound = false;
    if ( col[0] == "#" ) {
        col = col.slice(1);
        usePound = true;
    }

    var num = parseInt(col, 16);

    var r = (num >> 16) + amt;

    if ( r > 255 ) r = 255;
    else if  (r < 0) r = 0;

    var b = ((num >> 8) & 0x00FF) + amt;

    if ( b > 255 ) b = 255;
    else if  (b < 0) b = 0;

    var g = (num & 0x0000FF) + amt;

    if ( g > 255 ) g = 255;
    else if  ( g < 0 ) g = 0;

    return (usePound ? "#" : "") + (g | (b << 8) | (r << 16)).toString(16);
}

</script>


{#if isStatic === true}

<div class="card-text m-2 pt-1 px-3 text-capitalize chiclet {getSmallChicletClass()}"
  style="
  --type-color: {typeColor}; 
  --font-color:{getFontColor()};">
  <p>
    {typeName}
  </p>
</div>

{:else}

<div class="card-text m-2 pt-1 px-3 text-capitalize chiclet {getSmallChicletClass()} chiclet-btn"
  style="
    --type-color: {typeColor}; 
    --font-color:{getFontColor()}; 
    --type-color-hover:{lightenDarkenColor(typeColor, -20)}; 
    --type-color-active:{lightenDarkenColor(typeColor, -50)};"
    data-bs-toggle="modal" 
    data-bs-target="#{modalName}"
    data-bs-dismiss="modal">
  <p>
    {typeName}
  </p>
</div>

{/if}

<style>
  .chiclet {
    cursor: default;
    width: 5rem;
    height: 2rem;
    border-radius: 15px;
    text-align: center;
    font-size: small;
    background-color: var(--type-color);
    color: var(--font-color);
  }

  .chiclet-sm {
    padding: 0;
    width: 4rem;
    height: 1.5rem;
    font-size: x-small;
  }

  .chiclet-btn {
    cursor: pointer;
  }

  .chiclet-btn:hover {
    background-color: var(--type-color-hover);
  }

  .chiclet-btn:active {
    background-color: var(--type-color-active);
  }
</style>
