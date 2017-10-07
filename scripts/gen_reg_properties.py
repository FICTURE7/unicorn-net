import sys
import os

def main(args):
    if len(args) < 3:
        print("ERROR: Must specify file defining registers and prefix of register names in command line argument.", file=sys.stderr)
        print(os.path.basename(__file__) + " <filename> <register prefix>");
        exit(1)
    
    regs_prefix = args[2] + '_'
    regs_file = open(args[1], 'r')
    regs = regs_file.read().split(',')
    regs_file.close()

    regid = 1
    final_code = ''
    for reg in regs:
        reg = reg.strip()
		
        name = reg[reg.index(regs_prefix) + len(regs_prefix):]
        code = "/// <summary>\n/// Gets or sets the value of {0} register.\n/// </summary>\n".format(name)
        code += "public long {0} \n{{\n".format(name)
        code += "    get {{ return Read({0}); }}\n".format(regid)
        code += "    set {{ Write({0}, value); }}".format(regid)
        code += "\n}\n\n";

        regid += 1
        final_code += code

    code_file = open(args[1] + '.cs', 'w')
    code_file.write(final_code)

if __name__ == '__main__':
    main(sys.argv)
