
export class Utils {

    // Helper method that converts an uploaded file into a Base64 string
    public static async ToBase64(file : File) : Promise<string> {

        return new Promise<string>((encoded) => {
            var reader = new FileReader();
    
            reader.readAsDataURL(file);
            reader.onload = () => {
                encoded(<string>reader.result);
            }
        })
    }
}