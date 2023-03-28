export class CommentViewModel {
    id? : number;
    username! : string;
    subtitleMovieId? : number;
    subtitleTvShowId? : number;
    content! : string;
    page? : number;
    totalPages? : number;
    image? : string | null;
}