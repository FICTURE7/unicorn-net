import sys;

def main(args):
    if len(args) < 2:
        print("Must specified file defining registers in command line argument.", file=sys.stderr);
        exit(1);

    regs_file = open(args[1], 'r');
    regs = regs_file.read().split(',');
    regs_file.close();

    regid = 1;
    final_code = '';
    for reg in regs:
        reg = reg.strip();

        name = reg[reg.rindex('_') + 1:];
        code = '/// <summary>\n/// Gets or sets the value of ' + name + ' register.\n/// </summary>\n';
        code += 'public long ' + name + ' \n{\n';
        code += '    get {{ return Read({0}); }}\n'.format(regid);
        code += '    set {{ Write({0}, value); }}'.format(regid);
        code += '\n}\n\n';

        regid += 1;
        final_code += code;

    code_file = open(args[1] + '.cs', 'w');
    code_file.write(final_code);

if __name__ == '__main__':
    main(sys.argv);