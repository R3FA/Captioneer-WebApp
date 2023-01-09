export class TVShowViewModel {
    title: string = "";
    imdbId: string = "";
    synopsis: string = "";
    year: string = "";
    seasonCount? : number;
    episodeCount? : number;
    imdbRatingValue?: number;
    imdbRatingCount?: number;
    rottenTomatoesValue: string = "";
    metacriticValue: string = "";
    coverArt: string = "";
}