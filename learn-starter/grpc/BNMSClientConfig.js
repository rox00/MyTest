import path from 'path'
import { fileURLToPath } from 'url'

/** BNMSClientConfig class
 * this configuration just for BNMSClient
*/
export default class BNMSClientConfig {
    constructor() {
        const __filenameNew = fileURLToPath(import.meta.url)
        const __dirnameNew = path.dirname(__filenameNew)
        var PROTO_PATH = __dirnameNew + '/BNMSStatus.proto';

        this.servername = 'localhost';
        this.serverport = '8888';
        this.protofile = PROTO_PATH;
      }
}